using System.Collections.Generic;
using UnityEngine;

public enum CapybaraState
{
    Idle,
    Move,
    Dead,
    Attack1,
    Attack2,
}

public class CapybaraVisual : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private int idleHash = Animator.StringToHash("Idle");
    private int moveHash = Animator.StringToHash("Move");
    private int deadHash = Animator.StringToHash("Dead");
    private int attack1Hash = Animator.StringToHash("Skill1");
    private int attack2Hash = Animator.StringToHash("Skill2");

    private Dictionary<CapybaraState, int> hashTable = new();

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        hashTable.Add(CapybaraState.Idle, idleHash);
        hashTable.Add(CapybaraState.Move, moveHash);
        hashTable.Add(CapybaraState.Dead, deadHash);
        hashTable.Add(CapybaraState.Attack1, attack1Hash);
        hashTable.Add(CapybaraState.Attack2, attack2Hash);
        ChangeState(CapybaraState.Idle);
    }

    public void Selected()
    {

    }

    public void Unselected()
    {

    }

    public void ChangeState(CapybaraState state)
    {
        foreach (int hash in hashTable.Values)
            animator.SetBool(hash, false);

        animator.SetBool(hashTable[state], true);
    }
}
