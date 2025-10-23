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

        if (GameManager.Instance.IsFirstLaunch())
        {
            GameManager.Instance.MarkAsLaunched();

            GameManager.Instance.startPanel.gameObject.SetActive(false);
            GameManager.Instance.OpenInfoPanel();
        }
        else                                       
            GameManager.Instance.StartGame();
    }
}
