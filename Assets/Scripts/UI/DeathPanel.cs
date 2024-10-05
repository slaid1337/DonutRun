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

    private void Awake()
    {
        Instance = this;
    }

    public void Open()
    {
        FadeBG.Instance.Fade();
        OpenPanel();
    }

    public void Close()
    {
        FadeBG.Instance.UnFade();
        ClosePanel();
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
        GameTransition.Instance.OpenTransition(delegate
        {
            SceneManager.LoadScene("MainMenu");
        });
    }
}
