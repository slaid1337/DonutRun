using DonutRun;
using Eiko.YaSDK;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PausePanel : BasePanel
{
    public static PausePanel Instance { get; private set; }

    public UnityEvent OnPause;
    public UnityEvent OnResume;
    [SerializeField] private Donut _donut;

    private void Awake()
    {
        Instance = this;
    }

    public void Open()
    {
        FadeBG.Instance.Fade();
        OpenPanel();
        OnPause?.Invoke();
    }

    public void Close()
    {
        FadeBG.Instance.UnFade();
        ClosePanel();
        OnResume?.Invoke();
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