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
    private SearchEngineManager _searchEngineManager;

    public void Setup(SearchResultEntry entry)
    {
        Setup(entry, FindFirstObjectByType<SearchEngineManager>());
    }

    public void Setup(SearchResultEntry entry, SearchEngineManager searchEngineManager)
    {
        _entry = entry;
        _searchEngineManager = searchEngineManager;

        titleText.text = entry.title;
        descriptionText.text = entry.description;

        if (thumbnailImage != null && entry.thumbnail != null)
            thumbnailImage.sprite = entry.thumbnail;

        openButton.onClick.RemoveListener(OnClickOpen);
        openButton.onClick.AddListener(OnClickOpen);
    }

    void OnClickOpen()
    {
        if (_searchEngineManager != null)
            _searchEngineManager.OpenSearchResult(_entry);
    }
}
