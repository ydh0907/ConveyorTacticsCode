using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Obstacle : MonoBehaviour, ISetable, IMoveable, IHealth
{
    private Block position;
    [SerializeField] private int health = 5;

    public async UniTask Set(Block block)
    {
        position?.SetBlock(null);
        position = block;
        position.SetBlock(this);
        transform.position = position.transform.position + Vector3.up;
        transform.parent = position.transform;
        await transform.DOLocalMoveY(0, 0.2f).SetEase(Ease.OutBounce);
    }

    public async UniTask Move(Block block)
    {
        await Set(block);
    }

    public void ModifyHealth(int value)
    {
        health += value;
        if (health <= 0)
            Dead();
    }

    public void Dead()
    {
        position?.SetBlock(null);
        Destroy(gameObject);
    }

    public bool IsBlockMove { get; set; }

    public bool CanMove(Block block)
    {
        return false;
    }
}
