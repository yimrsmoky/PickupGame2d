using UnityEngine;
using UnityEngine.UI;

public class DonateButton : MonoBehaviour
{
    private Button Button;
    private string cloudTipsLink = "https://pay.cloudtips.ru/p/87e9ceb8";

    private void Start()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(OpenDonateLink);
    }
    void OpenDonateLink()
    {
        Application.OpenURL(cloudTipsLink);
    }
}
