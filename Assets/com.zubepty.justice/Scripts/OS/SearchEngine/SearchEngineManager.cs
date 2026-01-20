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
        if (searchInput.text != String.Empty)
        {
            string input = searchInput.text.ToLower().Trim();
            _SearchResultPage.SetActive(true);
            var data = allSearchResults.FirstOrDefault(r => r.queryKeyword.ToLower() == input);
            foreach (Transform child in resultsParent) Destroy(child.gameObject);

            if (data != null)
            {
                foreach (var entry in data.results)
                {
                    var go = Instantiate(resultPrefab, resultsParent);
                    go.GetComponent<ResultUI>().Setup(entry);
                }
            }
        }
      
    }
}
