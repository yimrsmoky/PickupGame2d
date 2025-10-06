using UnityEngine;

public class DontDestroyCanvas : MonoBehaviour
{
   public static DontDestroyCanvas Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
