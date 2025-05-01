using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CapybaraGift : Skill
{
    private Block point => Owner.Position;

    public override int Cost => Owner.Info.skill_1.activePower;

    private Block last;

    [SerializeField] private CinemachineCamera focusCam;
    [SerializeField] private int damage = 10;

    private CapybaraVisual visual;

    [SerializeField] private GiftBox GiftBox;
    [SerializeField] private Transform BoxTrm;

    private void Awake()
    {
        visual = GetComponent<CapybaraVisual>();
    }

    public override bool CanUse(Block block)
    {
        return block;
    }

    public override void UpdateData(Block block)
    {
        if (!block || block == last || OnSkill)
            return;
        ShowBlock(last, false);
        last = block;
        ShowBlock(last, true);

        Vector3 dir = block.transform.position - transform.position;
        Owner.transform.rotation = Quaternion.LookRotation(dir, transform.up);
    }

    protected override async UniTask OnUse(Block block)
    {
        ShowBlock(last, false);
        last = block;
        OnSkill = true;
        focusCam.Priority = 10;
        await UniTask.Delay(1000);

        visual.ChangeState(CapybaraState.Attack1);
        await UniTask.Delay(2600);
        Instantiate(GiftBox, BoxTrm).Explosion();
        await UniTask.Delay(800);

        var list = GetBlocks(block);
        foreach (var target in list)
        {
            if (target.Current is IHealth health)
            {
                health.ModifyHealth(-damage);
            }
        }
        await UniTask.Delay(1500);

        visual.ChangeState(CapybaraState.Idle);
        focusCam.Priority = 0;
        await UniTask.Delay(500);
        OnSkill = false;
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
        Vector2Int pos = point.Idx;

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0)
                    continue;
                Vector2Int temp = pos + new Vector2Int(i, j);
                Block block = MapManager.Instance.GetBlock(temp);
                if (block && !block.IsBlocked)
                    list.Add(block);
            }
        }

        return list;
    }
}
