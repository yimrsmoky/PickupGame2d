using UnityEngine;

public class Obstacles : MonoBehaviour
{
    private GameManager gameManager;

    public AudioClip crashSound;
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameManager.audioSource.PlayOneShot(crashSound);
        if (gameManager.lifes == 1)
        {
            gameManager.GameOver();
        }
        else
        {
            gameManager.UpdateLifes(1);
        }
    }
}
