using DonutRun;
using Eiko.YaSDK;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PausePanel : BasePanel
{
    public static PausePanel Instance { get; private set; }

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

    public void LoadMenu()
    {
        int currentScore = SaveController.Instance.GetBestScore();

        if (currentScore < _donut.Score)
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