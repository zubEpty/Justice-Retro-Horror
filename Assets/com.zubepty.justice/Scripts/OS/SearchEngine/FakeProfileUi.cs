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

    void Awake()
    {
        Instance = this;
        profilePanel.SetActive(false);
    }

    public void ShowProfile(FakeProfile profile)
    {
        nameText.text = profile.fullName;
        bioText.text = profile.bio;
        profileImage.sprite = profile.profileImage;

        foreach (Transform child in postsContainer)
            Destroy(child.gameObject);

        foreach (string post in profile.previousPosts)
        {
            var postObj = Instantiate(postPrefab, postsContainer);
            postObj.GetComponentInChildren<TextMeshProUGUI>().text = post;
        }

        profilePanel.SetActive(true);
    }
}
