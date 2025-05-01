using TMPro;
using UnityEngine;

public class MessageViewer : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text fullMessageText;

    public void ShowMessage(string message)
    {
        fullMessageText.text = message;
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
