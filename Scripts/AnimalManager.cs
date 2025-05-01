using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : TurnHandler
{
    public static AnimalManager Instance { get; private set; }

    public Animal Current { get; private set; }
    public Skill Skill { get; private set; }
    public Block Block { get; private set; }

    [field: SerializeField] public List<Animal> Animals { get; private set; }

    [SerializeField] private LayerMask CharacterLayer;
    [SerializeField] private LayerMask BlockLayer;

    public Action<Animal> OnCharacterSelected;
    public Action<Skill> OnSkillSelected;

    [SerializeField] private SkillPanel skillPanel;

    private bool isTurn = false;
    public bool Turn => isTurn;

    private void Awake()
    {
        Instance = this;
        skillPanel.skill1.onClick.AddListener(() => SkillChange(Current?.Skill1));
        skillPanel.skill2.onClick.AddListener(() => SkillChange(Current?.Skill2));
    }

    private void Update()
    {
        if (!isTurn) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        SelectBlock(ray);
        if (!SelectCharacter(ray))
            CharacterControl(ray).Forget();
        Block?.Select();
    }

    private void SelectBlock(Ray ray)
    {
        if (Block && Current && !Current.IsActing && !Skill && Current.CanMove(Block))
            SystemManager.Instance.activeBar.CancelBarValue();

        bool blockHit = Physics.Raycast(ray, out RaycastHit blockInfo, 1000, BlockLayer);
        Block?.Unselect();
        Block = blockInfo.transform?.GetComponent<Block>();

        if (Block && Current && !Current.IsActing && !Skill && Current.CanMove(Block))
            SystemManager.Instance.activeBar.CheckMinusBar(Current.MoveEnerge(Block));
    }

    private bool SelectCharacter(Ray ray)
    {
        if (!Current && Input.GetMouseButtonDown(0))
        {
            Physics.Raycast(ray, out RaycastHit characterInfo, 1000, CharacterLayer);
            Animal selected = characterInfo.transform?.GetComponent<Animal>();
            if (!selected)
            {
                Physics.Raycast(ray, out characterInfo, 1000, BlockLayer);
                selected = characterInfo.transform?.GetComponent<Block>()?.Current as Animal;
            }
            CharacterChange(selected);
            return true;
        }
        return false;
    }

    private async UniTask CharacterControl(Ray ray)
    {
        if (Input.GetMouseButtonDown(1) && Current && Skill)
        {
            SkillChange(null);
            return;
        }

        if (Current && !Current.IsActing)
        {
            Skill?.UpdateData(Block);

            if (Input.GetMouseButtonDown(0) && Block)
            {
                if (Skill)
                {
                    if (Skill.CanUse(Block) && !Skill.OnSkill)
                    {
                        if (Block.Current == Current)
                        {
                            SkillChange(null);
                            return;
                        }
                        if (SystemManager.Instance.activeBar.CheckMinusActiveValue())
                        {
                            await Skill.Use(Block);
                            SkillChange(null);
                        }
                        else
                            SkillChange(null);
                    }
                }
                else
                {
                    if (Block.Current != null)
                    {
                        if (Block.Current == Current)
                            CharacterChange(null);
                        else if (Block.Current is Animal next)
                            CharacterChange(next);
                    }
                    else if (Current.CanMove(Block))
                    {
                        if (SystemManager.Instance.activeBar.CheckMinusActiveValue())
                        {
                            SystemManager.Instance.activeBar.CompleteBarValue();
                            Current.Move(Block).Forget();
                        }
                        else
                            CharacterChange(null);
                    }
                }
            }
        }
    }

    private void CharacterChange(Animal character)
    {
        Current?.Unselect();
        Current = character;
        if (Current && !Current.Selectable)
            Current = null;
        Current?.Select();
        if (Current)
        {
            SystemManager.Instance.skillManager.SetPlayerInfo(Current.Info);
            SystemManager.Instance.activePanel.UpdatePlayerSkill();
        }
        SkillChange(null);
    }

    private void SkillChange(Skill skill)
    {
        if (Current != null && Current.IsActing)
            return;
        if (Skill)
        {
            if (Skill == skill)
            {
                SkillChange(null);
                return;
            }
            if (Skill && Skill.OnSkill)
                return;
        }
        Skill?.Unselect();
        Skill = skill;
        Skill?.Select();
    }

    public override async UniTask<bool> HandleTurn()
    {
        if (EnemyManager.Instance.IsGameClear())
        {
            Debug.Log("Game Clear Animal");
            SystemManager.Instance.endingSystem.GameClear();
            return false;
        }
        if (IsGameOver())
        {
            Debug.Log("Game Over Animal");
            SystemManager.Instance.endingSystem.GameOver();
            return false;
        }
        isTurn = true;
        Debug.Log("Animal Turn");
        SystemManager.Instance.turnPanel.ChangeTurn();
        await UniTask.WaitWhile(() => isTurn);
        return true;
    }

    public bool IsGameOver()
    {
        return Animals.Count == 0 && GameManager.Instance.isInitEnd;
    }

    public void EndTurn()
    {
        if (Current && Current.IsActing || Skill && Skill.OnSkill)
            return;

        Block?.Unselect();
        Block = null;
        SkillChange(null);
        CharacterChange(null);
        isTurn = false;
    }
}
