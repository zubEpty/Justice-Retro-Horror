using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PostUI : MonoBehaviour
{
    public TextMeshProUGUI postText;
    public Image postImage;

    public void Setup(ProfilePost post)
    {
        postText.text = post.postText;

        if (post.postImage != null)
        {
            postImage.gameObject.SetActive(true);
            postImage.sprite = post.postImage;
        }
        else
        {
            postImage.gameObject.SetActive(false);
        }
    }
}
