using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private TextMeshProUGUI fpsText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fpsText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        float fps = 1f / Time.deltaTime;
        fpsText.text = $"FPS: {Mathf.Round(fps)}";
    }
}
