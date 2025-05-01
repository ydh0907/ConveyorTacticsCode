using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class DogShootGun : Skill
{
    private enum localDir { left, up, right, down }

    public override int Cost => Owner.Info.skill_1.activePower;

    private Block point => Owner.Position;
    private bool[,,] area =
    {
        {
            { true, true, true},
            { false, false, false},
            { false, false, false}
        },
        {
            { false, false, true},
            { false, false, true},
            { false, false, true}
        },
        {
            { false, false, false},
            { false, false, false},
            { true, true, true}
        },
        {
            { true, false, false},
            { true, false, false},
            { true, false, false}
        },
    };
    private Vector2Int center = Vector2Int.one;

    private Block last;
    private DogVisual visual;

    [SerializeField] private CinemachineCamera focusCam;
    [SerializeField] GameObject gun;
    [SerializeField] private int damage = 20;

    private void Awake()
    {
        visual = GetComponent<DogVisual>();
    }

    public override bool CanUse(Block block)
    {
        return block;
    }

    public override void Select()
    {
        base.Select();
    }

    public override void Unselect()
    {
        UnshowArea();
        base.Unselect();
    }

    public override void UpdateData(Block block)
    {
        if (!block || block == last || OnSkill)
            return;
        UnshowArea();
        var dir = GetDir(block);
        ShowArea(GetBlocks(dir));
        Owner.transform.rotation = Quaternion.Euler(0, (int)dir * 90 + (dir == localDir.left || dir == localDir.right ? 180 : 0), 0);
        last = block;
    }

    private void ShowArea(List<Block> blocks)
    {
        if (blocks == null)
            return;
        foreach (var block in blocks)
        {
            block.SelectShow();
        }
    }

    private void UnshowArea()
    {
        Vector2Int startPos = point.Idx - center;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                MapManager.Instance.GetBlock(startPos.x + i, startPos.y + j)?.SelectUnshow();
            }
        }
    }

    private localDir GetDir(Block block)
    {
        if (block == null)
            return default;

        Vector2Int ownerPos = point.Idx;
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

    private List<Block> GetBlocks(localDir dir)
    {
        List<Block> blocks = new List<Block>();
        Vector2Int startPos = point.Idx - center;
        int dirInt = (int)dir;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (area[dirInt, i, j])
                {
                    Block block = MapManager.Instance.GetBlock(startPos.x + j, startPos.y + i);
                    if (block != null)
                        blocks.Add(block);
                }
            }
        }
        return blocks;
    }

    private int Abs(int value) => value >= 0 ? value : -value;

    protected override async UniTask OnUse(Block block)
    {
        OnSkill = true;
        UnshowArea();
        focusCam.Priority = 10;
        last = block;

        await UniTask.Delay(1000);
        gun.SetActive(true);
        visual.ChangeState(DogState.Shoot);
        await UniTask.Delay(2300);
        visual.ChangeState(DogState.Idle);
        gun.SetActive(false);
        await UniTask.Delay(500);

        focusCam.Priority = 0;
        OnSkill = false;
    }

    public void CastDamage()
    {
        List<Block> blocks = GetBlocks(GetDir(last));
        List<IHealth> targets = new();
        foreach (var blo in blocks)
            if (blo.Current is IHealth health)
                targets.Add(health);
        targets.ForEach((x) => x.ModifyHealth(-damage));
    }
}
