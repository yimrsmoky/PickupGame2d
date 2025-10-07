using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSavedLanguage();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetRussian() 
    { 
        StartCoroutine(ChangeLanguage("ru"));
        GameManager.Instance.CloseLangPanel();
    }
    public void SetEnglish()
    {
        StartCoroutine(ChangeLanguage("en"));
        GameManager.Instance.CloseLangPanel();
    }
    public void SetIngush()
    {
        StartCoroutine(ChangeLanguage("ing"));
        GameManager.Instance.CloseLangPanel();
    }

    IEnumerator ChangeLanguage(string code)
    {
        yield return LocalizationSettings.InitializationOperation;
        var locale = LocalizationSettings.AvailableLocales.GetLocale(code);

        LocalizationSettings.SelectedLocale = locale;

        PlayerPrefs.SetString("selected_language", code);
        PlayerPrefs.Save();
    }
    private void LoadSavedLanguage()
    {
        string savedLanguage = PlayerPrefs.GetString("selected_language", "ru");
        StartCoroutine(ChangeLanguage(savedLanguage));
    }
}
