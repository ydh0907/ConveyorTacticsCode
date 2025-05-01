using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemySkill/DefenderSkill")]
public class DefenderSkill : EnemySkill
{
    public ParticleSystem _bombParticle;
    public override async UniTask UseSkill()
    {

    }
}