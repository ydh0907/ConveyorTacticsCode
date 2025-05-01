using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public abstract class Item : MonoBehaviour, ISetable
{
    public async UniTask Set(Block block)
    {
        block.SetItem(this);
        transform.parent = block.transform;
        await transform.DOLocalMoveY(0, 0.2f).SetEase(Ease.OutBounce).ToUniTask();
    }

    public abstract UniTask Use(Animal animal);
}
