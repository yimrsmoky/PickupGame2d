using UnityEngine;

public class CargoPickup : MonoBehaviour
{
    private GameManager gameManager;
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
        Destroy(gameObject);
        gameManager.AddScore(1);
    }
}
