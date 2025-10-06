using UnityEngine;
using TMPro;
using System.Collections;

public class LocalizedText : MonoBehaviour
{
    public string key;
    private TextMeshProUGUI textComponent;
    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        StartCoroutine(DelayedUpdateText());
    }
    IEnumerator DelayedUpdateText()
    {
        yield return null;
        yield return new WaitForEndOfFrame();
        UpdateText();
    }
    public void UpdateText()
    {
        textComponent.text = Localization.Instance.GetText(key);
    }
}
