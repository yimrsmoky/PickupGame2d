using UnityEngine;
using UnityEngine.UI;

public class VkButton : MonoBehaviour
{
    private Button Button;
    private string vkLink = "https://vk.com/vaydev";

    private void Start()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(OpenVkLink);
    }
    void OpenVkLink()
    {
        Application.OpenURL(vkLink);
    }
}
