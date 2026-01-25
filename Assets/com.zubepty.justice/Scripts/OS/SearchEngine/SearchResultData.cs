using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SearchResultData", menuName = "FakeSearch/SearchResult")]
public class SearchResultData : ScriptableObject
{
    [Header("Keywords that trigger this result")]
    public List<string> queryKeywords; 
    public List<SearchResultEntry> results;
}

[System.Serializable]
public class SearchResultEntry
{
    public string title;
    public string description;
    public Sprite thumbnail;

    [Header("Navigation")]
    public SearchResultTarget target;

    [Header("Target Data")]
    public FakeProfile profile;        // Facebook
    public NewsArticleData newsData;    // News
    public string fakeUrl;

}

public enum SearchResultTarget
{
    FacebookProfile,
    NewsArticle,
    MissingReport,
    CCTVPage,
    Error404
}
