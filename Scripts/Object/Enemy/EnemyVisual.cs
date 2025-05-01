using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Move,
    Attack,
    Dead
}

public class EnemyVisual : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private int idleHash = Animator.StringToHash("Idle");
    private int moveHash = Animator.StringToHash("Move");
    private int attackHash = Animator.StringToHash("Attack");
    private int deadHash = Animator.StringToHash("Dead");

    private Dictionary<EnemyState, int> hashTable = new();

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        hashTable.Add(EnemyState.Idle, idleHash);
        hashTable.Add(EnemyState.Move, moveHash);
        hashTable.Add(EnemyState.Attack, attackHash);
        hashTable.Add(EnemyState.Dead, deadHash);
        ChangeState(EnemyState.Idle);
    }

    public void Selected()
    {

    }

    public void Unselected()
    {

    }

    public void ChangeState(EnemyState state)
    {
        foreach (int hash in hashTable.Values)
            animator.SetBool(hash, false);

        animator.SetBool(hashTable[state], true);
    }
}
