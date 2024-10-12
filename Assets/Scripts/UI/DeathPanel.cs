using DonutRun;
using Eiko.YaSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DeathPanel : BasePanel
{
    public static DeathPanel Instance;
    public UnityEvent OnRevive;
    [SerializeField] private Donut _donut;
    public UnityEvent OnPause;
    public UnityEvent OnResume;

    private void Awake()
    {
        Instance = this;
    }

    public void Open()
    {
        FadeBG.Instance.Fade();
        OpenPanel();
        OnPause?.Invoke();

        YandexSDK.StopAPI();
        YandexSDK.instance.CanPlay = false;
    }

    public void Close()
    {
        FadeBG.Instance.UnFade();
        ClosePanel();
        OnResume?.Invoke();

        YandexSDK.instance.CanPlay = true;
        YandexSDK.StartAPI();
    }

    public void Revive()
    {
        YandexSDK.instance.onRewardedAdReward += OnReward;
        
        YandexSDK.instance.ShowRewarded("Revive");
    }

    public void OnReward(string str)
    {
        OnRevive?.Invoke();
        Close();
    }
    public void Back()
    {
        int currentScore = SaveController.Instance.GetBestScore();

        if ( currentScore < _donut.Score)
        {
            SaveController.Instance.SetBestScore(_donut.Score);
            YandexSDK.instance.SetScore(_donut.Score);
        }

        GameTransition.Instance.OpenTransition(delegate
        {
            SceneManager.LoadScene("MainMenu");
        });
    }
}
