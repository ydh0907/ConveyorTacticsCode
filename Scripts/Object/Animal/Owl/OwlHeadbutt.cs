using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class OwlHeadbutt : Skill
{
    public override int Cost => Owner.Info.skill_2.activePower;

    private Block point => Owner.Position;
    private Block last;

    [SerializeField] private CinemachineCamera focusCam;
    [SerializeField] private int damage = 8;

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
        Vector2Int pos = point.Idx;

        for (int i = 0; i < 6; i++)
        {
            pos += dir;
            Block block = MapManager.Instance.GetBlock(pos);
            if (block)
                list.Add(block);
        }

        return list;
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

        visual.ChangeState(OwlState.Attack2);
        await UniTask.Delay(833);
        var list = GetBlocks(block);

        foreach (var target in list)
        {
            if (target.Current is IHealth health)
            {
                health.ModifyHealth(-damage);
            }
        }

        await UniTask.Delay(500);
        visual.ChangeState(OwlState.Idle);
        focusCam.Priority = 0;
        OnSkill = false;
    }
}
