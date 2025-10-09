using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private GameManager gameManager;

    private Button button;
    void Start()
    {
        gameManager = GameObject.FindFirstObjectByType<GameManager>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnPlayClick);
    }
    public void OnPlayClick()
    {
        int selectedLevel = GameManager.Instance.SelectedLevel;
        GameManager.Instance.StartGame();
    }
}
