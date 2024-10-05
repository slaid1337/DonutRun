using UnityEngine;
using Eiko.YaSDK.Data;
using System;

public class SaveController : Singletone<SaveController>
{
    public static Action<bool> OnMainAudioChenge;
    public static Action<bool> OnMusicAudioChenge;

    public void SetMoney(int count)
    {
        YandexPrefs.SetInt("Money", count);
    }

    public int GetMoney()
    {
        int money = YandexPrefs.GetInt("Money", 0);

        return money;
    }

    public void SetBestScore(int score)
    {
        YandexPrefs.SetInt("BestScore", score);
    }

    public int GetBestScore()
    {
        int score = YandexPrefs.GetInt("BestScore", 0);

        return score;
    }

    public void SetMusicAudio(bool isActive)
    {
        YandexPrefs.SetInt("MusicAudio", isActive ? 1 : 0);
    }

    public bool GetMusicAudio()
    {
        bool isActive = YandexPrefs.GetInt("MusicAudio", 0) == 1 ? true : false;

        return isActive;
    }

    public void SetMainAudio(bool isActive)
    {
        YandexPrefs.SetInt("MainAudio", isActive ? 1 : 0);
    }

    public bool GetMainAudio()
    {
        bool isActive = YandexPrefs.GetInt("MainAudio", 0) == 1 ? true : false;

        return isActive;
    }

    public void SetActiveDonut(string name)
    {
        YandexPrefs.SetString("DonutSkin", name);
    }

    public string GetActiveDonut()
    {
        string name = YandexPrefs.GetString("DonutSkin", "Gentleness");

        return name;
    }

    public void PurchaseItem(string name)
    {
        YandexPrefs.SetInt("Item" + name, 1);
    }

    public bool IsItemPurchased(string name)
    {
        bool isActive = YandexPrefs.GetInt("Item" + name, 0) == 1 ? true : false;

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
