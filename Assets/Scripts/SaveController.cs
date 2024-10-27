using UnityEngine;
using System;
using CGWebPlatform;

public class SaveController : Singletone<SaveController>
{
    public static Action<bool> OnMainAudioChenge;
    public static Action<bool> OnMusicAudioChenge;

    public void SetMoney(int count)
    {
        PlayerPrefs.SetInt("Money", count);
    }

    public int GetMoney()
    {
        int money = PlayerPrefs.GetInt("Money", 0);

        return money;
    }

    public void SetBestScore(int score)
    {
        PlayerPrefs.SetInt("BestScore", score);
    }

    public int GetBestScore()
    {
        int score = PlayerPrefs.GetInt("BestScore", 0);

        return score;
    }

    public void SetMusicAudio(bool isActive)
    {
        PlayerPrefs.SetInt("MusicAudio", isActive ? 1 : 0);
    }

    public bool GetMusicAudio()
    {
        bool isActive = PlayerPrefs.GetInt("MusicAudio", 0) == 1 ? true : false;

        return isActive;
    }

    public void SetMainAudio(bool isActive)
    {
        PlayerPrefs.SetInt("MainAudio", isActive ? 1 : 0);
    }

    public bool GetMainAudio()
    {
        bool isActive = PlayerPrefs.GetInt("MainAudio", 0) == 1 ? true : false;

        return isActive;
    }

    public void SetActiveDonut(string name)
    {
        PlayerPrefs.SetString("DonutSkin", name);
    }

    public string GetActiveDonut()
    {
        string name = PlayerPrefs.GetString("DonutSkin", "Gentleness");

        return name;
    }

    public void PurchaseItem(string name)
    {
        PlayerPrefs.SetInt("Item" + name, 1);
    }

    public bool IsItemPurchased(string name)
    {
        bool isActive = PlayerPrefs.GetInt("Item" + name, 0) == 1 ? true : false;

        return isActive;
    }

    public static bool IsMuteMainAudio()
    {
        return PlayerPrefs.GetInt("AudioMainMute", 0) == 1;
    }

    public static bool IsMuteMusicAudio()
    {
        return PlayerPrefs.GetInt("AudioMusicMute", 0) == 1;
    }

    public static void MuteMainAudio(bool isMute)
    {
        int result = 0;

        if (isMute) result = 1;
        else result = 0;

        PlayerPrefs.SetInt("AudioMainMute", result);

        OnMainAudioChenge?.Invoke(isMute);
    }

    public static void MuteMusicAudio(bool isMute)
    {
        int result = 0;

        if (isMute) result = 1;
        else result = 0;

        PlayerPrefs.SetInt("AudioMusicMute", result);
        OnMusicAudioChenge?.Invoke(isMute);
    }

    public void SetLanguage(string lang)
    {
        PlayerPrefs.SetString("LanguageLocal", lang);
    }

    public string GetLanguage()
    {
        return PlayerPrefs.GetString("LanguageLocal", "");
    }
}
