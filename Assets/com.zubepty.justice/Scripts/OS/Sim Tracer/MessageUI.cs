using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MessageUI : MonoBehaviour
{
    public TextMeshProUGUI previewText;
    private string fullMessage;

    // Reference to message viewer
    private MessageViewer messageViewer;

    public void Setup(string message, MessageViewer viewer)
    {
        fullMessage = message;
        previewText.text = GetPreview(message);
        messageViewer = viewer;

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        messageViewer.ShowMessage(fullMessage);
    }

    private string GetPreview(string full)
    {
        return full.Length > 30 ? full.Substring(0, 30) + "..." : full;
    }
}
