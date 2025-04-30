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
    public List<SearchResultData> allSearchResults;

    public void OnSearch()
    {
        string input = searchInput.text.ToLower().Trim();
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
        else
        {
            // Show no result or static list
        }
    }
}
