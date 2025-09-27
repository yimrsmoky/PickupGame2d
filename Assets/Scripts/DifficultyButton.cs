using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    public int difficulty;
    private GameManager gameManager;

    private Button button;
    void Start()
    {
        gameManager = GameObject.FindFirstObjectByType<GameManager>();
        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty);
    }
    public void SetDifficulty()
    {
        gameManager.StartGame(difficulty);
    }
}
