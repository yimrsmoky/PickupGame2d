using UnityEngine;

public class Obstacles : MonoBehaviour
{
    private GameManager gameManager;
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
        gameManager.UpdateLifes(1);
    }
}
