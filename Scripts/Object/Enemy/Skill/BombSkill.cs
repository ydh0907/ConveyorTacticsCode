using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemySkill/BombSkill")]
public class BombSkill : EnemySkill
{
    [SerializeField] private ParticleSystem _particleSystem;

    public override async UniTask UseSkill()
    {

    }
}