using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class OwlScissors : Skill
{
    public override int Cost => Owner.Info.skill_1.activePower;
    private Block point => Owner.Position;
    private Dictionary<Vector2Int, Vector2Int> angle90 = new Dictionary<Vector2Int, Vector2Int>()
    {
        { Vector2Int.up, Vector2Int.right },
        { Vector2Int.right, Vector2Int.down },
        { Vector2Int.down, Vector2Int.left },
        { Vector2Int.left, Vector2Int.up }
    };
    private Block last;

    [SerializeField] private CinemachineCamera focusCam;

    private OwlVisual visual;

    private void Awake()
    {
        visual = GetComponent<OwlVisual>();
    }

    public override bool CanUse(Block block)
    {
        return block;
    }

    public override void Select()
    {
        base.Select();
        last = point;
        ShowBlock(last, true);
    }

    public override void Unselect()
    {
        ShowBlock(last, false);
        base.Unselect();
    }

    public override void UpdateData(Block block)
    {
        if (!block || block == last || OnSkill)
            return;
        ShowBlock(last, false);
        last = block;
        ShowBlock(last, true);

        Vector3 dir = block.transform.position - transform.position;
        dir.y = 0;
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
            dir.z = 0;
        else
            dir.x = 0;
        Owner.transform.rotation = Quaternion.LookRotation(dir, transform.up);
    }

    protected override async UniTask OnUse(Block block)
    {
        Vector2Int dir = GetDir(block);
        ShowBlock(last, false);
        last = block;
        OnSkill = true;
        focusCam.Priority = 10;
        await UniTask.Delay(1000);

        visual.ChangeState(OwlState.Attack1);
        await UniTask.Delay(1100);
        var list = GetBlocks(block);

        foreach (var target in list)
        {
            if (target.Current is IMoveable moveable)
            {
                Block moveTo = target;
                Vector2Int temp = target.Idx;
                Debug.Log(temp);
                for (int i = 0; i < 3; i++)
                {
                    temp += dir;
                    Block next = MapManager.Instance.GetBlock(temp);
                    if (next != null && next.Current == null)
                    {
                        moveTo = next;
                    }
                    else break;
                }
                Debug.Log(temp);

                moveable.Move(moveTo).Forget();
            }
        }

        await UniTask.Delay(500);
        visual.ChangeState(OwlState.Idle);
        await UniTask.Delay(500);

        focusCam.Priority = 0;
        OnSkill = false;
    }

    private Vector2Int GetDir(Block block)
    {
        if (block == null)
            return default;

        Vector2Int ownerPos = point.Idx;
        Vector2Int targetPos = block.Idx;
        int diffY = targetPos.x - ownerPos.x;
        int diffX = targetPos.y - ownerPos.y;
        if (Mathf.Abs(diffX) >= Mathf.Abs(diffY))
        {
            if (diffX >= 0)
                return Vector2Int.up;
            else
                return Vector2Int.down;
        }
        else
        {
            if (diffY >= 0)
                return Vector2Int.right;
            else
                return Vector2Int.left;
        }
    }

    public void ShowBlock(Block target, bool enable)
    {
        if (!target)
            return;

        var list = GetBlocks(target);
        foreach (var block in list)
            if (enable)
                block.SelectShow();
            else
                block.SelectUnshow();
    }

    private List<Block> GetBlocks(Block target)
    {
        var list = new List<Block>();

        Vector2Int dir = GetDir(target);
        Vector2Int ver = angle90[dir];
        Vector2Int pos = point.Idx;
        pos += dir * 3;

        for (int i = 0; i < 3; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Vector2Int t = pos + ver * j;
                Block block = MapManager.Instance.GetBlock(t);

                if (block)
                {
                    list.Add(block);
                }
            }
            pos -= dir;
        }

        return list;
    }
}
