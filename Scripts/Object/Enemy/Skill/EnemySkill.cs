using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class EnemySkill : ScriptableObject
{
    protected Enemy owner;
    public abstract UniTask UseSkill();
    public virtual void SetOwner(Enemy enemy)
    {
        owner = enemy;
    }
}