using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemyInfo")]
public class EnemyInfoSO : ScriptableObject
{
    public int Health;
    public int Damage;
    public int Defense;
    
    public bool IsAllowPenetration;

    public bool IsAdditionalDamage;
    public int AdditionalDamagePercent;
    
    public RangeInfoSO RangeInfo;
}