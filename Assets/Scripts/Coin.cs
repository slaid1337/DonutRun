using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    public int Revenue;

    private void Start()
    {
        transform.DOLocalRotate(new Vector3(0, 360f, 0), 2f, RotateMode.LocalAxisAdd).SetLoops(-1).SetEase(Ease.Linear);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
