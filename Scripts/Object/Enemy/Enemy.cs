using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, ISetable, IMoveable
{
    [field: SerializeField] public EnemyInfoSO Info { get; private set; }
    [field: SerializeField] public EnemySkill Skill { get; private set; }
    public EnemyHealth Health { get; private set; }
    protected EnemyVisual Visual { get; private set; }
    public Block position { get; set; }
    public bool IsBlockMove { get; set; } = false;

    protected virtual void Awake()
    {
        Visual = GetComponent<EnemyVisual>();
        Health = GetComponent<EnemyHealth>();
        Health?.SetOwner(this);
    }

    protected virtual void Start()
    {
        EnemyManager.Instance.enemies.Add(this);
    }

    protected virtual void OnDestroy()
    {
        EnemyManager.Instance?.enemies.Remove(this);
        EnergyManager.Instance.KillEnemyCount++;
    }

    public abstract UniTask HandleTurn();
    public abstract UniTask Set(Block block);
    public bool CanMove(Block block)
    {
        if (IsBlockMove) return false;
        return block;
    }

    public abstract UniTask Move(Block block);
}
