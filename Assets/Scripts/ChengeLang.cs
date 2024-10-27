using UnityEngine;
using Lean.Localization;

public class ChengeLang : MonoBehaviour
{
    private void Start()
    {
        OnLoad();
    }

    public void OnLoad()
    {
        string currentLang = SaveController.Instance.GetLanguage();

        if (currentLang == "")
        {
            currentLang = "EN";
        }

        LeanLocalization.SetCurrentLanguageAll(currentLang);
        Debug.Log(currentLang);
    }

    public void SetLangauge(string lang)
    {
        lang = lang.ToUpper();
        LeanLocalization.SetCurrentLanguageAll(lang);
        SaveController.Instance.SetLanguage(lang);
    }
}