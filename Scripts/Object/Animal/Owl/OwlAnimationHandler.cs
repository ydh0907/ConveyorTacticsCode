using UnityEngine;

public class OwlAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animal owner;

    public void Destroy()
    {
        Destroy(owner.gameObject);
    }
}
