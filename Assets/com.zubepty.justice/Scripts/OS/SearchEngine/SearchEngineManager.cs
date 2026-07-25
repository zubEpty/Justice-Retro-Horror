using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using TMPro;

public class SearchEngineManager : MonoBehaviour
{
    public TMP_InputField searchInput;
    public Transform resultsParent;
    public GameObject resultPrefab;
    public GameObject NoResultPage;
    public List<SearchResultData> allSearchResults;

    [SerializeField] private TextMeshProUGUI _uriText;
    [SerializeField] private GameObject _SearchResultPage;
    [SerializeField] private GameObject _homePage;
    [SerializeField] private Button _backButton;

    [Header("Browser Pages")]
    [SerializeField] private GameObject[] _browserPages;

    [Header("Search Shortcuts")]
    [SerializeField] private Button _gmtkPageButton;
    [SerializeField] private GameObject _gmtkPage;
    [SerializeField] private string _gmtkUrl = "gmtk.com";

    [SerializeField] private Button _antivirusPageButton;
    [SerializeField] private GameObject _antivirusPage;
    [SerializeField] private string _antivirusUrl = "antivirus.com";

    [SerializeField] private Button _freakbookPageButton;
    [SerializeField] private FakeProfile _jaberFreakbookProfile;
    [SerializeField] private string _jaberFreakbookUrl = "freakbook.com/jabermolla425";

    private bool _shortcutsInitialized;

    private void Awake()
    {
        InitializeShortcuts();
    }

    private void OnEnable()
    {
        InitializeShortcuts();
    }

    private void InitializeShortcuts()
    {
        if (_shortcutsInitialized)
            return;

        if (_gmtkPageButton != null)
            _gmtkPageButton.onClick.AddListener(OpenGmtkPage);

        if (_antivirusPageButton != null)
            _antivirusPageButton.onClick.AddListener(OpenAntivirusPage);

        if (_freakbookPageButton != null)
            _freakbookPageButton.onClick.AddListener(OpenJaberFreakbookPage);

        if (_backButton != null)
            _backButton.onClick.AddListener(GoBackToHomePage);

        _shortcutsInitialized = true;
    }

    public void ClearEntry()
    {
        _uriText.text = string.Empty;
        searchInput.text = string.Empty;
    }

    public void OnSearch()
    {
        string input = searchInput.text.ToLower().Trim();

        if (string.IsNullOrEmpty(input))
            return;

        if (_homePage != null)
            _homePage.SetActive(false);

        HideBrowserPages();
        _SearchResultPage.SetActive(true);
        _uriText.text = input;

        // Start fake browser loading
        BrowserLoadingUi.Instance.Load(() =>
        {
            // Clear old results AFTER loading
            foreach (Transform child in resultsParent)
                Destroy(child.gameObject);

            // Find matching search data
            SearchResultData matchedData = allSearchResults.FirstOrDefault(data =>
                data.queryKeywords.Any(keyword =>
                    input.Contains(keyword.ToLower())
                )
            );

            if (matchedData != null)
            {
                NoResultPage.SetActive(false);

                foreach (var entry in matchedData.results)
                {
                    var go = Instantiate(resultPrefab, resultsParent);
                    go.GetComponent<ResultUI>().Setup(entry);
                }
            }
            else
            {
                NoResultPage.SetActive(true);
            }
        });

    }

    public void OpenGmtkPage()
    {
        LoadStaticPage(_gmtkPage, _gmtkUrl);
    }

    public void OpenAntivirusPage()
    {
        LoadStaticPage(_antivirusPage, _antivirusUrl);
    }

    public void OpenJaberFreakbookPage()
    {
        if (_jaberFreakbookProfile == null)
            return;

        SetUrl(_jaberFreakbookUrl);

        BrowserLoadingUi.Instance.Load(() =>
        {
            HideBrowserPages();

            if (_homePage != null)
                _homePage.SetActive(false);

            if (_SearchResultPage != null)
                _SearchResultPage.SetActive(true);

            if (NoResultPage != null)
                NoResultPage.SetActive(false);

            FakeProfileUI.Instance.ShowProfile(
                _jaberFreakbookProfile,
                _jaberFreakbookUrl
            );
        });
    }

    public void GoBackToHomePage()
    {
        if (IsHomePageOnlyActive())
            return;

        HideBrowserPages();

        if (_homePage != null)
            _homePage.SetActive(true);

        ClearEntry();
    }

    private void LoadStaticPage(GameObject page, string url)
    {
        if (page == null)
            return;

        SetUrl(url);

        BrowserLoadingUi.Instance.Load(() =>
        {
            HideBrowserPages();

            if (_homePage != null)
                _homePage.SetActive(false);

            page.SetActive(true);

            FakeNewsUI newsPage = page.GetComponent<FakeNewsUI>();
            if (newsPage != null)
                newsPage.ShowPage();
        });
    }

    private void HideBrowserPages()
    {
        if (_browserPages == null)
            return;

        foreach (GameObject page in _browserPages)
        {
            if (page != null)
                page.SetActive(false);
        }
    }

    private bool IsHomePageOnlyActive()
    {
        if (_homePage == null || !_homePage.activeSelf)
            return false;

        if (_browserPages == null)
            return true;

        foreach (GameObject page in _browserPages)
        {
            if (page != null && page.activeSelf)
                return false;
        }

        return true;
    }

    private void SetUrl(string url)
    {
        if (_uriText != null)
            _uriText.text = url;

        if (searchInput != null)
            searchInput.text = string.Empty;
    }
}
