using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FakeProfileUI : MonoBehaviour
{
    public static FakeProfileUI Instance;

    public GameObject profilePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI bioText;
    public Image profileImage;
    public Transform postsContainer;
    public GameObject postPrefab;
    public TextMeshProUGUI urlText;

    public GameObject NoResultPage;

    void Awake()
    {
        Instance = this;
        profilePanel.SetActive(false);
    }

    public void ShowProfile(FakeProfile profile, string fakeUrl)
    {
        nameText.text = profile.fullName;
        bioText.text = profile.bio;
        profileImage.sprite = profile.profileImage;

        if (urlText != null)
            urlText.text = fakeUrl;

        foreach (Transform child in postsContainer)
            Destroy(child.gameObject);

        foreach (var post in profile.previousPosts)
        {
            var postObj = Instantiate(postPrefab, postsContainer);
            postObj.GetComponent<PostUI>().Setup(post);
        }

        profilePanel.SetActive(true);
    }

    public void ShowErrorResultsUi(string url)
    {
        NoResultPage.SetActive(true);
        urlText.text = url;
    }

}
