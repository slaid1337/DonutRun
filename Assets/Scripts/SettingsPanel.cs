using Lean.Localization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsPanel : BasePanel
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _mainAudioSlider;
    [SerializeField] private Image ENOutline;
    [SerializeField] private Image RUOutline;
    [SerializeField] private bool _isPlayScene;
    private float _musicSliderLastValue = 3;
    private float _mainSliderLastValue = 3;
    public UnityEvent OnPause;
    public UnityEvent OnResume;
    public static SettingsPanel Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Open()
    {
        FadeBG.Instance.Fade();
        OpenPanel();
        RefreshBtns();
        RefreshLang();
        OnPause?.Invoke();
    }

    public void RefreshLang()
    {
        string lang = SaveController.Instance.GetLanguage();

        if (lang == "")
        {
            lang = "EN";
        }

        if (lang == "RU")
        {
            ENOutline.enabled = false;
            RUOutline.enabled = true;
        }
        else
        {
            ENOutline.enabled = true;
            RUOutline.enabled = false;
        }
    }

    public void Close()
    {
        FadeBG.Instance.UnFade();
        ClosePanel();
        RefreshBtns();
        OnResume?.Invoke();
    }

    public void SetRussian()
    {
        LeanLocalization.SetCurrentLanguageAll("RU");
        SaveController.Instance.SetLanguage("RU");
        RefreshLang();
    }

    public void SetEnglish()
    {
        LeanLocalization.SetCurrentLanguageAll("EN");
        SaveController.Instance.SetLanguage("EN");
        RefreshLang();
    }

    private void RefreshBtns()
    {
        bool isMuteMusic = SaveController.IsMuteMusicAudio();

        if (isMuteMusic)
        {
            _musicSlider.value = 0;
        }
        else
        {
            _musicSlider.value = 1;
        }

        bool isMutemain = SaveController.IsMuteMainAudio();

        if (isMutemain)
        {
            _mainAudioSlider.value = 0;
        }
        else
        {
            _mainAudioSlider.value = 1;
        }
    }

    public void ToggleMainAudio()
    {
        if (_mainAudioSlider.value == _mainSliderLastValue) return;
        
        if (_mainAudioSlider.value == 0)
        {
            SoundController.Instance.MuteMainSounds();
        }
        else
        {
            SoundController.Instance.UnmuteMainSounds();
        }

        _mainSliderLastValue = _mainAudioSlider.value;

        RefreshBtns();
    }

    public void ToggleMusicAudio()
    {
        if (_musicSlider.value == _musicSliderLastValue) return;

        if (_musicSlider.value == 0)
        {
            SoundController.Instance.MuteMusic();
        }
        else
        {
            SoundController.Instance.UnmuteMusic();
        }

        _musicSliderLastValue = _musicSlider.value;

        RefreshBtns();
    }
}
