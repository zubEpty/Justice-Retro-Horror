using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public Image thumbnailImage;
    public Button openButton;
 
    private SearchResultEntry _entry;

    public void Setup(SearchResultEntry entry)
    {
        _entry = entry;

        titleText.text = entry.title;
        descriptionText.text = entry.description;

        if (thumbnailImage != null && entry.thumbnail != null)
            thumbnailImage.sprite = entry.thumbnail;

        openButton.onClick.AddListener(OnClickOpen);
    }

    void OnClickOpen()
    {
        if (_entry.leadsToProfile && _entry.profile != null)
        {
            // Open fake social media profile
            FakeProfileUI.Instance.ShowProfile(_entry.profile, _entry.fakeUrl);
        }
        else
        {
            FakeProfileUI.Instance.ShowErrorResultsUi(_entry.fakeUrl);
        }
    }
}
