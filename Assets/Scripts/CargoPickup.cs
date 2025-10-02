using UnityEngine;

public class CargoPickup : MonoBehaviour
{
    private GameManager gameManager;

    public AudioClip pickupSound;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CollectCargo();
        }
    }
    void CollectCargo()
    {
        int score = gameManager.score;
        int scoreToWin = gameManager.scoreToWin;
        Destroy(gameObject);
        gameManager.audioSource.PlayOneShot(pickupSound);
        gameManager.AddScore(1);
        if (score == scoreToWin)
        {
            //gameManager.LevelCompleted();
            gameManager.ToNextLevel();
        }
        else if (score != scoreToWin)
        {
            gameManager.SpawnCargo();
        }
    }
}
