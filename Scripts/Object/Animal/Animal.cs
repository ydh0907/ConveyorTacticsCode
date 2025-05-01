using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class Animal : MonoBehaviour, ISetable, IMoveable, IHealth, ISelectable, ISkillUse
{
    public abstract Skill Skill1 { get; set; }
    public abstract Skill Skill2 { get; set; }
    public abstract bool IsActing { get; }
    
    public abstract Block Position { get; }
    [field: SerializeField] public bool Selectable { get; protected set; } = true;
    [field: SerializeField] public PlayerInfoSO Info { get; protected set; }

    protected virtual void Start()
    {
        Skill1?.SetOwner(this);
        Skill2?.SetOwner(this);
        AnimalManager.Instance.Animals.Add(this);
    }

    protected virtual void OnDestroy()
    {
        AnimalManager.Instance?.Animals.Remove(this);
        if (EnergyManager.Instance)
            EnergyManager.Instance.DeadPlayer++;
        Debug.Log("Dead");
    }

    public abstract UniTask Set(Block block);
    public bool IsBlockMove { get; set; }
    public abstract bool CanMove(Block block);
    public abstract int MoveEnerge(Block block);
    public abstract UniTask Move(Block block);
    public abstract void ShowMovable(bool show);
    public abstract void Select();
    public abstract void Unselect();
    public abstract void ModifyHealth(int value);
    public abstract void Dead();
}
