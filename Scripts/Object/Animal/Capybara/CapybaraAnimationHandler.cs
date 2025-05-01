using UnityEngine;

public class CapybaraAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animal owner;

    public void Destroy()
    {
        Destroy(owner.gameObject);
    }
}
