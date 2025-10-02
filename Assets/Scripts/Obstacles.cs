using UnityEngine;

public class Obstacles : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.TouchingAnObstacle();
    }
}
