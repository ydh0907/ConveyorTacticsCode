using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class DogRide : Skill
{
    public override int Cost => Owner.Info.skill_2.activePower;
    private enum localDir { left, up, right, down }
    private Vector2Int[] dirSet = { Vector2Int.down, Vector2Int.right, Vector2Int.up, Vector2Int.left };

    private Block last = null;
    private IHealth target = null;

    private DogVisual visual;

    [SerializeField] private CinemachineCamera focusCam;
    [SerializeField] private int damage = 5;

    private void Awake()
    {
        visual = GetComponent<DogVisual>();
    }

    public override bool CanUse(Block block)
    {
        return true;
    }

    public override void Select()
    {
        base.Select();
    }

    public override void Unselect()
    {
        UnshowDir(GetDir(last));
        base.Unselect();
    }

    public override void UpdateData(Block block)
    {
        if (block == null || block == last || OnSkill)
            return;
        localDir lastDir = GetDir(last);
        UnshowDir(lastDir);
        localDir currDir = GetDir(block);
        ShowDir(currDir);
        Owner.transform.rotation = Quaternion.Euler(0, (int)currDir * 90 + (currDir == localDir.left || currDir == localDir.right ? 180 : 0), 0);
        last = block;
    }

    protected override async UniTask OnUse(Block block)
    {
        OnSkill = true;
        UnshowDir(GetDir(block));

        focusCam.Priority = 10;
        await UniTask.Delay(1000);

        var blocks = GetBlocks(GetDir(block));
        Block moveTo = Owner.Position;
        Block target = null;
        for (int i = 0; i < blocks.Count; i++)
        {
            if (blocks[i].Current != null)
            {
                target = blocks[i];
                break;
            }
            else
                moveTo = blocks[i];
        }
        this.target = target?.Current as IHealth;
        await Owner.Move(moveTo);
        Owner.ShowMovable(false);

        if (target)
        {
            visual.ChangeState(DogState.JumpAttack);
            await UniTask.Delay(2000);
        }

        visual.ChangeState(DogState.Idle);

        focusCam.Priority = 0;
        await UniTask.Delay(1000);
        OnSkill = false;
    }

    private void ShowDir(localDir dir)
    {
        var blocks = GetBlocks(dir);
        foreach (var block in blocks)
            block.SelectShow();
    }

    private void UnshowDir(localDir dir)
    {
        var blocks = GetBlocks(dir);
        foreach (var block in blocks)
            block.SelectUnshow();
    }

    public void CastDamage()
    {
        target?.ModifyHealth(-damage);
    }

    private List<Block> GetBlocks(localDir dir)
    {
        int dirInt = (int)dir;
        Vector2Int offset = Owner.Position.Idx;
        List<Block> blocks = new();
        for (int i = 1; i <= 3; i++)
        {
            Vector2Int pos = dirSet[(int)dir] * i + offset;
            Block block = MapManager.Instance.GetBlock(pos);
            if (block != null)
                blocks.Add(block);
        }
        return blocks;
    }

    private localDir GetDir(Block block)
    {
        if (block == null)
            return default;

        Vector2Int ownerPos = Owner.Position.Idx;
        Vector2Int targetPos = block.Idx;
        int diffY = targetPos.x - ownerPos.x;
        int diffX = targetPos.y - ownerPos.y;
        localDir dir;
        if (Abs(diffX) >= Abs(diffY))
            if (diffX >= 0)
                dir = localDir.right;
            else
                dir = localDir.left;
        else
        {
            if (diffY >= 0)
                dir = localDir.up;
            else
                dir = localDir.down;
        }

        return dir;
    }

    private int Abs(int value) => value >= 0 ? value : -value;
}
