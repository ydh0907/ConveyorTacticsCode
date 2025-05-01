using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform bar;
    private Camera main;

    private void Awake()
    {
        main = Camera.main;
    }

    private void Update()
    {
        transform.forward = main.transform.forward;
    }

    public void Set(float per)
    {
        var t = bar.localScale;
        t.x = per;
        bar.localScale = t;
    }
}
