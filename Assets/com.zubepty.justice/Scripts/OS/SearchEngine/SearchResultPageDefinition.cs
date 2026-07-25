using UnityEngine;

[CreateAssetMenu(fileName = "SearchResultPage", menuName = "FakeSearch/Page")]
public class SearchResultPageDefinition : ScriptableObject
{
    [SerializeField] private SearchResultPageKind pageKind = SearchResultPageKind.ScenePage;

    public SearchResultPageKind PageKind => pageKind;
}

public enum SearchResultPageKind
{
    ScenePage,
    Profile,
    NewsArticle,
    ErrorPage
}
