using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MessageUI : MonoBehaviour
{
    public TMP_Text previewText;
    private MessageData msgData;
    private MessageViewer messageViewer;

    public void Setup(MessageData data, MessageViewer viewer)
    {
        msgData = data;
        messageViewer = viewer;

        previewText.text = GetPreview(data.content);
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        messageViewer.ShowMessage(msgData);
    }

    private string GetPreview(string full)
    {
        return full.Length > 30 ? full.Substring(0, 30) + "..." : full;
    }
}
