using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FakeNewsUI : MonoBehaviour
{
    public static FakeNewsUI Instance;

    [Header("Root")]
    [SerializeField] private GameObject rootPanel;

    [Header("UI Elements")]
    [SerializeField] private Image newsImage;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI urlText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        rootPanel.SetActive(false);
    }

    public void OpenArticle(NewsArticleData data, string fakeUrl)
    {
        if (data == null)
            return;

        newsImage.sprite = data.newsImage;
        titleText.text = data.title;
        descriptionText.text = data.description;
        urlText.text = fakeUrl;

        rootPanel.SetActive(true);
    }

    public void Close()
    {
        rootPanel.SetActive(false);
    }
}