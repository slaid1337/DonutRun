using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeBG : Singletone<FadeBG>
{
    private Image _image;

    public void Fade(float speed = 0.5f)
    {
        _image = GetComponent<Image>();
        _image.raycastTarget = true;
        _image.DOColor(new Color(0f, 0f, 0f, 0.8f), speed);
    }

    public void UnFade(float speed = 0.5f)
    {
        _image = GetComponent<Image>();
        _image.raycastTarget = false;
        _image.DOColor(new Color(0f, 0f, 0f, 0f), speed);
    }
}
