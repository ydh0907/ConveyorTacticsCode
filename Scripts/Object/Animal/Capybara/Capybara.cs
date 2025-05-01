using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Capybara : Animal
{
    [SerializeField] private int moveEnerge = 10;
    [field: SerializeField] public override Skill Skill1 { get; set; }
    [field: SerializeField] public override Skill Skill2 { get; set; }

    private bool isMoving = false;
    public override bool IsActing
    {
        get
        {
            return isMoving;
        }
    }

    private Block position;
    public override Block Position => position;

    private bool[,] movable = {
        { false, true, true, true, false },
        { true, true, true, true, true },
        { true, true, false, true, true },
        { true, true, true, true, true },
        { false, true, true, true, false },
    };
    private Vector2Int center = Vector2Int.one * 2;

    private AnimalHealth health;
    private CapybaraVisual visual;

    private void Awake()
    {
        health = GetComponent<AnimalHealth>();
        visual = GetComponent<CapybaraVisual>();
    }

    public override bool CanMove(Block block)
    {
        if (IsBlockMove) return false;
        if (block == null)
            return false;
        if (block.IsBlocked) return false;
        Vector2Int delta = block.Idx - position.Idx;
        bool isMoveable = false;
        if (Abs(delta.x) <= center.x && Abs(delta.y) <= center.y)
        {
            delta += center;
            isMoveable = movable[delta.y, delta.x];
        }
        isMoveable = isMoveable && block.Current == null;
        return isMoveable;
    }

    public override void Dead()
    {
        position?.SetBlock(null);
        Selectable = false;
        visual.ChangeState(CapybaraState.Dead);
        EnergyManager.Instance.DeadPlayer++;
        AnimalManager.Instance.Animals.Remove(this);
    }

    public override void ModifyHealth(int value)
    {
        health.ModifyHealth(value);
    }

    public override async UniTask Move(Block block)
    {
        isMoving = true;
        Vector3 dir = block.transform.position - transform.position;
        await transform.DORotate(new Vector3(0, Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg, 0), 0.3f).ToUniTask();
        visual.ChangeState(CapybaraState.Move);
        ShowMovable(false);
        await transform.DOMove(block.transform.position, 1.5f).ToUniTask();
        visual.ChangeState(CapybaraState.Idle);
        await Set(block);
        if (block.Item)
            await block.Item.Use(this);
        ShowMovable(true);
        isMoving = false;
    }

    public override void Select()
    {
        visual.Selected();
        ShowMovable(true);
    }

    public override async UniTask Set(Block block)
    {
        position?.SetBlock(null);
        position = block;
        position?.SetBlock(this);
        transform.parent = position?.transform;
        await transform.DOLocalMove(Vector3.zero, 0.2f).SetEase(Ease.OutBounce).ToUniTask();
    }

    public override void ShowMovable(bool show)
    {
        if (!AnimalManager.Instance.Turn || AnimalManager.Instance.Current != this)
            return;
        Vector2Int startPos = position.Idx - center;
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 7; y++)
            {
                Vector2Int temp = startPos + new Vector2Int(x, y);
                Block block = MapManager.Instance.GetBlock(temp);
                if (CanMove(block))
                {
                    if (show)
                        block.SelectShow();
                    else
                        block.SelectUnshow();
                }
            }
        }
    }

    public override void Unselect()
    {
        ShowMovable(false);
        visual.Unselected();
    }

    private int Abs(int value) => value >= 0 ? value : -value;

    public override int MoveEnerge(Block block)
    {
        if (CanMove(block))
            return moveEnerge;
        return 0;
    }
}
