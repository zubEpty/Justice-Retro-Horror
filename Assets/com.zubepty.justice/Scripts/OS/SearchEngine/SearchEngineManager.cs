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
    [SerializeField] private SearchResultPageRegistry _pageRegistry;

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
                    go.GetComponent<ResultUI>().Setup(entry, this);
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

    public void OpenBrowserPage(GameObject page, string url)
    {
        LoadStaticPage(page, url);
    }

    public void OpenSearchResult(SearchResultEntry entry)
    {
        if (entry == null)
            return;

        BrowserLoadingUi.Instance.Load(() =>
        {
            if (entry.page != null)
            {
                OpenPageDefinition(entry);
                return;
            }

            OpenLegacyTarget(entry);
        });
    }

    public void GoBackToHomePage()
    {
        if (IsHomePageOnlyActive())
            return;

        ResetToHomePage();
    }

    public void ResetToHomePage()
    {
        HideBrowserPages();

        if (_homePage != null)
            _homePage.SetActive(true);

        if (_SearchResultPage != null)
            _SearchResultPage.SetActive(false);

        if (NoResultPage != null)
            NoResultPage.SetActive(false);

        if (resultsParent != null)
        {
            foreach (Transform child in resultsParent)
                Destroy(child.gameObject);
        }

        ClearEntry();
    }

    private void LoadStaticPage(GameObject page, string url)
    {
        if (page == null)
            return;

        SetUrl(url);

        BrowserLoadingUi.Instance.Load(() =>
        {
            ShowStaticPage(page);
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

    private void OpenPageDefinition(SearchResultEntry entry)
    {
        switch (entry.page.PageKind)
        {
            case SearchResultPageKind.Profile:
                OpenProfile(entry);
                break;

            case SearchResultPageKind.NewsArticle:
                OpenNewsArticle(entry);
                break;

            case SearchResultPageKind.ScenePage:
                OpenRegisteredScenePage(entry);
                break;

            case SearchResultPageKind.ErrorPage:
            default:
                OpenErrorPage(entry);
                break;
        }
    }

    private void OpenLegacyTarget(SearchResultEntry entry)
    {
        switch (entry.target)
        {
            case SearchResultTarget.FacebookProfile:
                OpenProfile(entry);
                break;

            case SearchResultTarget.NewsArticle:
                OpenNewsArticle(entry);
                break;

            case SearchResultTarget.Error404:
            default:
                OpenErrorPage(entry);
                break;
        }
    }

    private void OpenRegisteredScenePage(SearchResultEntry entry)
    {
        if (_pageRegistry == null || !_pageRegistry.TryGetPage(entry.page, out GameObject page, out string defaultUrl))
        {
            OpenErrorPage(entry);
            return;
        }

        string url = string.IsNullOrWhiteSpace(entry.fakeUrl) ? defaultUrl : entry.fakeUrl;
        SetUrl(url);
        ShowStaticPage(page);
    }

    private void OpenProfile(SearchResultEntry entry)
    {
        SetUrl(entry.fakeUrl);
        HideBrowserPages();

        if (_homePage != null)
            _homePage.SetActive(false);

        if (NoResultPage != null)
            NoResultPage.SetActive(false);

        if (entry.profile != null)
            FakeProfileUI.Instance.ShowProfile(entry.profile, entry.fakeUrl);
        else
            OpenErrorPage(entry);
    }

    private void OpenNewsArticle(SearchResultEntry entry)
    {
        if (entry.newsData == null)
        {
            OpenErrorPage(entry);
            return;
        }

        SetUrl(entry.fakeUrl);
        HideBrowserPages();

        if (_homePage != null)
            _homePage.SetActive(false);

        if (NoResultPage != null)
            NoResultPage.SetActive(false);

        FakeNewsUI.Instance.OpenArticle(entry.newsData, entry.fakeUrl);
    }

    private void OpenErrorPage(SearchResultEntry entry)
    {
        SetUrl(entry.fakeUrl);
        HideBrowserPages();

        if (_homePage != null)
            _homePage.SetActive(false);

        FakeProfileUI.Instance.ShowErrorResultsUi(entry.fakeUrl);
    }

    private void ShowStaticPage(GameObject page)
    {
        HideBrowserPages();

        if (_homePage != null)
            _homePage.SetActive(false);

        if (NoResultPage != null)
            NoResultPage.SetActive(false);

        page.SetActive(true);

        FakeNewsUI newsPage = page.GetComponent<FakeNewsUI>();
        if (newsPage != null)
            newsPage.ShowPage();
    }
}
