using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI promptText;

    public void Show(string message)
    {
        panel.SetActive(true);
        promptText.text = message;
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}