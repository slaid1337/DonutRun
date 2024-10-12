using DG.Tweening;
using Eiko.YaSDK;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameTransition : Singletone<GameTransition>
{
    [SerializeField] private RectTransform _transition;
    [SerializeField] private bool _waitLoad;
    [SerializeField] private bool _isPlayScene;
    public UnityEvent OnLoad;

    private void Start()
    {
        if (_waitLoad)
        {
            CloseTransition(delegate
            {
                YandexSDK.Ready();
            });
        }
        else if (_isPlayScene)
        {
            CloseTransition(delegate
            {
                OnLoad?.Invoke();
                YandexSDK.StartAPI();
            });
        }
        else
        {
            CloseTransition();
        }
    }

    public void OpenTransition(Action onComplete)
    {
        _transition.gameObject.SetActive(true);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_transition.GetComponent<Image>().DOFade(1f, 1.5f));
        sequence.Join(_transition.DOLocalRotate(new Vector3(0, 0, -360f), 1.5f, RotateMode.FastBeyond360));
        sequence.onComplete += delegate
        {
            YandexSDK.instance.ShowInterstitial();
            onComplete.Invoke();
        };
    }

    public void CloseTransition()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(_transition.GetComponent<Image>().DOFade(0f, 1.5f));
        sequence.Join(_transition.DOLocalRotate(new Vector3(0, 0, -360f), 1.5f, RotateMode.FastBeyond360));
        sequence.onComplete += delegate
        {
            _transition.gameObject.SetActive(false);
        };
    }

    public void CloseTransition(Action onComplete)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(_transition.GetComponent<Image>().DOFade(0f, 1.5f));
        sequence.Join(_transition.DOLocalRotate(new Vector3(0, 0, -360f), 1.5f, RotateMode.FastBeyond360));
        sequence.onComplete += delegate
        {
            _transition.gameObject.SetActive(false);
            onComplete.Invoke();
        };
    }
}
