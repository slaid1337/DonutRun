using CGWebPlatform;
using DonutRun;
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
        CGWebPlatformSDK.Instance.OnGetAdReward.AddListener(OnReward);
    }

    private void OnDestroy()
    {
        CGWebPlatformSDK.Instance.OnGetAdReward.RemoveListener(OnReward);
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

    public void Revive()
    {
        CGWebPlatformSDK.Instance.ShowRewarded("Revive");
    }

    public void OnReward(string str)
    {
        if (str != "Revive") return;
        OnRevive?.Invoke();
        Close();
    }
    public void Back()
    {
        int currentScore = SaveController.Instance.GetBestScore();

        if (currentScore < _donut.Score)
        {
            SaveController.Instance.SetBestScore(_donut.Score);

        }

        GameTransition.Instance.OpenTransition(delegate
        {
            SceneManager.LoadScene("MainMenu");
        });

    }
}
