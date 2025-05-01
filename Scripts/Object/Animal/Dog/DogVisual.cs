using System.Collections.Generic;
using UnityEngine;


public enum DogState
{
    Idle,
    Move,
    Shoot,
    JumpAttack,
    Dead
}

public class DogVisual : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private int idleHash = Animator.StringToHash("Idle");
    private int moveHash = Animator.StringToHash("Move");
    private int shootHash = Animator.StringToHash("Shoot");
    private int jumpAttackHash = Animator.StringToHash("JumpAttack");
    private int deadHash = Animator.StringToHash("Dead");

    private Dictionary<DogState, int> hashTable = new();

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        hashTable.Add(DogState.Idle, idleHash);
        hashTable.Add(DogState.Move, moveHash);
        hashTable.Add(DogState.Shoot, shootHash);
        hashTable.Add(DogState.JumpAttack, jumpAttackHash);
        hashTable.Add(DogState.Dead, deadHash);
        ChangeState(DogState.Idle);
    }

    public void Selected()
    {

    }

    public void Unselected()
    {

    }

    public void ChangeState(DogState state)
    {
        foreach (int hash in hashTable.Values)
            animator.SetBool(hash, false);

        animator.SetBool(hashTable[state], true);
    }
}
