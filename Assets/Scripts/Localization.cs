using System.Collections.Generic;
using UnityEngine;

public class Localization : MonoBehaviour
{
    public static Localization Instance;

    public enum Language { Russian, Ingush, English}
    public Language currentLanguage = Language.Russian;

    [System.Serializable]

    public class LocalizedText
    {
        public string key;
        public string russian;
        public string ingush;
        public string english;
    }

    public List<LocalizedText> texts = new List<LocalizedText>();
    private Dictionary<string, LocalizedText> textsDict;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void InitializeDictionary()
    {
        textsDict = new Dictionary<string, LocalizedText>();
        foreach (var text in texts)
        {
            textsDict[text.key] = text;
        }
    }
    public string GetText(string key)
    {
        if (textsDict.ContainsKey(key))
        {
            var text = textsDict[key];
            switch (currentLanguage)
            {
                case Language.Russian: return text.russian;
                case Language.Ingush: return text.ingush;
                case Language.English: return text.english;
            }
        }
        return $"[{key}]";
    }
}
