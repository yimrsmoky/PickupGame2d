using UnityEngine;

public class CargoPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            GameManager.Instance.CollectCargo();
        }
    }
}
