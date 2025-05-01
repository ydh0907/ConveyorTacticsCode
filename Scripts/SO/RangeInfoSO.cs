using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/RangeInfoSO"), Serializable]
public class RangeInfoSO : ScriptableObject
{
    public enum rangedir
    {
        left, up, right, down
    }

    public int MoveRange = 3;
    public bool[] MoveBlock;
    public int AttackRange = 3;
    public bool[] AttackBlock;

    private Vector2Int center = Vector2Int.one * 2;

    private void OnEnable()
    {
        SetMoveArray();
        SetAttackArray();
    }

    public void SetMoveArray()
    {
        if (MoveBlock == null || MoveBlock.Length != MoveRange * MoveRange)
            MoveBlock = new bool[MoveRange * MoveRange];
    }

    public void SetAttackArray()
    {
        if (AttackBlock == null || AttackBlock.Length != AttackRange * AttackRange)
            AttackBlock = new bool[AttackRange * AttackRange];
    }

    public List<Block> GetMoveableBlocks(Block position)
    {
        List<Block> list = new();
        Vector2Int pos = position.Idx;
        int centerIdx = (MoveRange - 1) / 2;
        for (int i = 0; i < MoveRange; i++)
        {
            for (int j = 0; j < MoveRange; j++)
            {
                int idx = i * MoveRange + j;
                Vector2Int t = pos + new Vector2Int(i - centerIdx, j - centerIdx);
                Block block = MapManager.Instance.GetBlock(t);
                if (block && block.Current == null && MoveBlock[idx])
                    list.Add(block);
            }
        }
        return list;
    }

    public void ShowMovealbe(List<Block> list, bool show)
    {
        // foreach (Block block in list)
        //     if (show)
        //         block.SelectShow();
        //     else
        //         block.SelectUnshow();
    }

    public List<Block> GetAttackableBlocks(Block position, rangedir dir)
    {
        List<Block> list = new();
        Vector2Int pos = position.Idx;
        bool[] rotated = RotateList(AttackBlock, (int)dir, AttackRange);
        int centerIdx = (AttackRange - 1) / 2;
        for (int i = 0; i < AttackRange; i++)
        {
            for (int j = 0; j < AttackRange; j++)
            {
                Vector2Int t = pos + new Vector2Int(i - centerIdx, j - centerIdx);
                Block block = MapManager.Instance.GetBlock(t);
                if (block && rotated[i * AttackRange + j])
                {
                    list.Add(block);
                }
            }
        }
        return list;
    }

    public static bool[] RotateList(bool[] origin, int dir, int range)
    {
        bool[] res = new bool[range * range];
        Array.Copy(origin, res, range * range);
        bool[] tep = new bool[range * range];
        for (int c = 0; c < dir; c++)
        {
            for (int i = 0; i < range; i++)
                for (int j = 0; j < range; j++)
                {
                    tep[j * range + range - 1 - i] = res[i * range + j];
                }
            bool[] t = tep;
            tep = res;
            res = t;
        }
        return res;
    }

    private int Abs(int value) => value >= 0 ? value : -value;
}
