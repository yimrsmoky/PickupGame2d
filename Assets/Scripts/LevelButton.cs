using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [Header("Настройки")]
    public int levelNumber;

    [Header("Визуал")]
    public Color selectedColor = Color.yellow;
    public Color normalColor = Color.white;
    public Color lockedColor = Color.gray;

    [Header("Элементы")]
    public TextMeshProUGUI levelText;
    public GameObject lockIcon;

    private Button button;
    private Image buttonImage;

    void Start()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();

        levelText.text = levelNumber.ToString();
        button.onClick.AddListener(OnLevelClick);

        UpdateAppearance();
    }
    void OnLevelClick()
    {
        GameManager.Instance.SelectedLevel = levelNumber;

        PlayerPrefs.SetInt("last_selected_level", levelNumber);
        PlayerPrefs.Save();
        SceneManager.LoadScene(levelNumber - 1);
        GameManager.Instance.UpdateAllLevelButtons();
    }
    public void UpdateAppearance()
    {
        int unlockedLevel = PlayerPrefs.GetInt("unlocked_level", 1);
        bool isUnlocked = levelNumber <= unlockedLevel;
        bool isSelected = GameManager.Instance.SelectedLevel == levelNumber;

        button.interactable = isUnlocked;
        buttonImage.color = isSelected ? selectedColor : normalColor;
        lockIcon.SetActive(!isUnlocked);

        if (isUnlocked)
        {
            buttonImage.color = isSelected ? selectedColor : normalColor;
        }
        else
        {
            buttonImage.color = lockedColor;

        }
    }
}
