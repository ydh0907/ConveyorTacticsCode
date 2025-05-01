using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Melee : Enemy, IMoveable
{    
    private Block position;

    public override async UniTask HandleTurn()
    {
        var pos = MapManager.Instance.GetIdx(position);
        pos.x -= 1;
        Block block = MapManager.Instance.GetBlock(pos);
        if (block == null)
            return;
        if (CanMove(block))
            await Move(block);
    }

    public override async UniTask Move(Block block)
    {
        await transform.DOMove(block.transform.position + Vector3.up, 0.5f).ToUniTask();
        await Set(block);
    }

    public override async UniTask Set(Block block)
    {
        position?.SetBlock(null);
        position = block;
        position?.SetBlock(this);
        if (position == null)
            return;
        transform.position = position.transform.position + Vector3.up;
        transform.parent = position.transform;
        await transform.DOLocalMoveY(0, 0.2f).SetEase(Ease.OutBounce).ToUniTask();
    }
}