using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Dog : Animal
{
    [SerializeField] private int moveEnerge = 10;
    [SerializeField] private float moveTime = 0.3f;

    [SerializeField] private AnimalHealth health;
    [SerializeField] private DogVisual visual;

    [field: SerializeField] public override Skill Skill1 { get; set; }
    [field: SerializeField] public override Skill Skill2 { get; set; }

    private Block position;
    public override Block Position => position;

    private bool[,] movable =
{
        { true, true, true },
        { true, false, true },
        { true, true, true },
    };
    private Vector2Int center = Vector2Int.one;

    private bool isMoving = false;
    public override bool IsActing
    {
        get
        {
            return isMoving;
        }
    }

    private void Awake()
    {
        health = GetComponent<AnimalHealth>();
        visual = GetComponent<DogVisual>();
        Skill1 = GetComponent<DogShootGun>();
        Skill2 = GetComponent<DogRide>();
    }

    public override void Dead()
    {
        position?.SetBlock(null);
        Selectable = false;
        visual.ChangeState(DogState.Dead);
        EnergyManager.Instance.DeadPlayer++;
        AnimalManager.Instance.Animals.Remove(this);
    }

    public override void ModifyHealth(int value)
    {
        health.ModifyHealth(value);
    }

    public override void ShowMovable(bool show)
    {
        if (!AnimalManager.Instance.Turn || AnimalManager.Instance.Current != this)
            return;
        Vector2Int startPos = position.Idx - center;
        int maxX = MapManager.Instance.maxLine;
        int maxY = MapManager.Instance.maxBlock;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
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

    public override async UniTask Move(Block block)
    {
        if (position == block)
            return;

        isMoving = true;
        Vector3 dir = block.transform.position - transform.position;
        await transform.DORotate(new Vector3(0, Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg, 0), 0.3f).ToUniTask();
        visual.ChangeState(DogState.Move);
        ShowMovable(false);
        await transform.DOMove(block.transform.position, moveTime).ToUniTask();
        visual.ChangeState(DogState.Idle);
        await Set(block);
        if (block.Item)
            await block.Item.Use(this);
        ShowMovable(true);
        isMoving = false;
    }

    private int Abs(int value) => value >= 0 ? value : -value;

    public override async UniTask Set(Block block)
    {
        position?.SetBlock(null);
        position = block;
        position?.SetBlock(this);
        transform.parent = position?.transform;
        await transform.DOLocalMove(Vector3.zero, 0.2f).SetEase(Ease.OutBounce).ToUniTask();
    }

    public override void Select()
    {
        visual.Selected();
        ShowMovable(true);
    }

    public override void Unselect()
    {
        visual?.Unselected();
        ShowMovable(false);
    }

    public override int MoveEnerge(Block block)
    {
        if (CanMove(block))
            return moveEnerge;
        return 0;
    }
}
