using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SearchResultData", menuName = "FakeSearch/SearchResult")]
public class SearchResultData : ScriptableObject
{
    public string queryKeyword;
    public List<SearchResultEntry> results;
}

[System.Serializable]
public class SearchResultEntry
{
    public string title;
    public string description;
    public Sprite thumbnail;
    public bool leadsToProfile;
    public FakeProfile profile; // link to a profile if this is the correct entry
}
