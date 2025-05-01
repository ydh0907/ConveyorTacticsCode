using UnityEngine;

public class GiftBox : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Explosion() // 0.8f
    {
        animator.SetTrigger("Boom");
        Destroy(gameObject, 1.2f);
    }
}
