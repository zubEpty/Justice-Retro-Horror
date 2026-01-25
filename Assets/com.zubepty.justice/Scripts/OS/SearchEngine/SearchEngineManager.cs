using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.Events;

public class SearchEngineManager : MonoBehaviour
{
    public TMP_InputField searchInput;
    public Transform resultsParent;
    public GameObject resultPrefab;
    public GameObject NoResultPage;
    public List<SearchResultData> allSearchResults;

    [SerializeField] private TextMeshProUGUI _uriText;
    [SerializeField] private GameObject _SearchResultPage;
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
}
