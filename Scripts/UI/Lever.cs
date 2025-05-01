using DG.Tweening;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Tween LeftMoveLever()
    {
        return gameObject.transform.DORotate(new Vector3(0, 0, 60), 0.5f);
    }

    public Tween RightMoveLever()
    {
        return gameObject.transform.DORotate(new Vector3(0, 0, -60), 0.5f);
    }
}
