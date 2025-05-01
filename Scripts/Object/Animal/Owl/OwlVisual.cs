using System.Collections.Generic;
using UnityEngine;

public enum OwlState
{
    Idle,
    Move,
    Dead,
    Attack1,
    Attack2,
}

public class OwlVisual : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private int idleHash = Animator.StringToHash("Idle");
    private int moveHash = Animator.StringToHash("Move");
    private int deadHash = Animator.StringToHash("Dead");
    private int attack1Hash = Animator.StringToHash("Attack1");
    private int attack2Hash = Animator.StringToHash("Attack2");

    private Dictionary<OwlState, int> hashTable = new();

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        hashTable.Add(OwlState.Idle, idleHash);
        hashTable.Add(OwlState.Move, moveHash);
        hashTable.Add(OwlState.Dead, deadHash);
        hashTable.Add(OwlState.Attack1, attack1Hash);
        hashTable.Add(OwlState.Attack2, attack2Hash);
        ChangeState(OwlState.Idle);
    }

    public void Selected()
    {

    }

    public void Unselected()
    {

    }

    public void ChangeState(OwlState state)
    {
        foreach (int hash in hashTable.Values)
            animator.SetBool(hash, false);

        animator.SetBool(hashTable[state], true);
    }
}
