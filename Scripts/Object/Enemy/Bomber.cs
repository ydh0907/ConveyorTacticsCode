using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using static RangeInfoSO;

public class Bomber : Enemy, IMoveable, IHealth
{
    public float moveSpeed = 2f;

    protected override void Start()
    {
        base.Start();
        Skill.SetOwner(this);
    }
    public override async UniTask HandleTurn()
    {
        Animal animal = GetCloseAnimal();

        Block moveTo = GetCloseBlock(animal, Info.RangeInfo.GetMoveableBlocks(position));
        await Move(moveTo);

        rangedir dir = GetAttackDir(animal);
        List<Block> attackRange = Info.RangeInfo.GetAttackableBlocks(position, dir);
        if (CanAttack(animal, attackRange))
            await Attack(animal, attackRange);
    }
    private async UniTask Attack(Animal target, List<Block> range)
    {
        if (target == null) return;

        Vector3 dir = target.transform.position - transform.position;
        await transform.DORotate(new Vector3(0, Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg, 0), 0.3f).ToUniTask();

        Visual.ChangeState(EnemyState.Attack);
        await UniTask.Delay(500);
        target.ModifyHealth(-Info.Damage);
        await UniTask.Delay(300);
        Visual.ChangeState(EnemyState.Idle);
    }
    private rangedir GetAttackDir(Animal target)
    {
        if (target == null) return rangedir.up;

        Vector2Int ownerPos = position.Idx;
        Vector2Int targetPos = target.Position.Idx;
        int diffY = targetPos.x - ownerPos.x;
        int diffX = targetPos.y - ownerPos.y;
        if (Mathf.Abs(diffX) >= Mathf.Abs(diffY))
        {
            if (diffX >= 0)
                return rangedir.up;
            else
                return rangedir.down;
        }
        else
        {
            if (diffY >= 0)
                return rangedir.right;
            else
                return rangedir.left;
        }
    }
    private bool CanAttack(Animal target, List<Block> range)
    {
        if (target == null) return false;

        foreach (var block in range)
        {
            Debug.Log(position.Idx);
            Debug.Log(block.Idx);
            if (target == block.Current && !block.IsBlocked)
                return true;
        }
        return false;
    }
    private Animal GetCloseAnimal()
    {
        var list = AnimalManager.Instance.Animals;
        Animal close = null;
        float dis = float.MaxValue;
        foreach (var animal in list)
        {
            float tdis = Vector3.Distance(animal.transform.position, transform.position);
            if (tdis < dis)
            {
                dis = tdis;
                close = animal;
            }
        }
        return close;
    }
    private Block GetCloseBlock(Animal animal, List<Block> list)
    {
        if (!animal) return null;

        Block close = null;
        float dis = float.MaxValue;
        foreach (var block in list)
        {
            float tdis = Vector3.Distance(animal.transform.position, block.transform.position);
            if (tdis < dis)
            {
                close = block;
                dis = tdis;
            }
        }
        return close;
    }
    public override async UniTask Set(Block block)
    {
        position?.SetBlock(null);
        position = block;
        position?.SetBlock(this);
        if (position != null)
        {
            transform.parent = position.transform;
            transform.localPosition = Vector3.up;
        }
        await transform.DOLocalMoveY(0, 0.2f).SetEase(Ease.OutBounce).ToUniTask();
    }
    public override async UniTask Move(Block block)
    {
        if (!block || !CanMove(block))
            return;
        Vector3 dir = block.transform.position - transform.position;
        await transform.DORotate(new Vector3(0, Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg, 0), 0.3f).ToUniTask();
        Vector3 pos = block.transform.position;

        Visual.ChangeState(EnemyState.Move);
        await transform.DOMove(pos, 0.3f).ToUniTask();
        Visual.ChangeState(EnemyState.Idle);
        await Set(block);
    }
    public void ModifyHealth(int value)
    {
        Debug.Log("Bomber Hit");
        Health.ModifyHealth(value);
    }
    public void Dead()
    {
        Health.Dead();
    }
}