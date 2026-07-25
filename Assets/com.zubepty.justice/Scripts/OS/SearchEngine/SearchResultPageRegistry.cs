using System;
using System.Collections.Generic;
using UnityEngine;

public class SearchResultPageRegistry : MonoBehaviour
{
    [SerializeField] private List<SearchResultPageBinding> pageBindings = new List<SearchResultPageBinding>();

    public bool TryGetPage(SearchResultPageDefinition definition, out GameObject page, out string defaultUrl)
    {
        foreach (SearchResultPageBinding binding in pageBindings)
        {
            if (binding.Definition == definition)
            {
                page = binding.Page;
                defaultUrl = binding.DefaultUrl;
                return page != null;
            }
        }

        page = null;
        defaultUrl = string.Empty;
        return false;
    }
}

[Serializable]
public class SearchResultPageBinding
{
    [SerializeField] private SearchResultPageDefinition definition;
    [SerializeField] private GameObject page;
    [SerializeField] private string defaultUrl;

    public SearchResultPageDefinition Definition => definition;
    public GameObject Page => page;
    public string DefaultUrl => defaultUrl;
}
