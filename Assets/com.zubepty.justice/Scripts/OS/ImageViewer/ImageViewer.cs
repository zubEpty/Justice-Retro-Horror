using UnityEngine;
using UnityEngine.UI;

public class ImageViewer : MonoBehaviour
{
    public static ImageViewer Instance;

    public Image fullImage;
    public GameObject windowRoot;

    void Awake()
    {
        Instance = this;
        windowRoot.SetActive(false);
    }

    public void ShowImage(Sprite sprite)
    {
        fullImage.sprite = sprite;
        windowRoot.SetActive(true);
        windowRoot.transform.SetAsLastSibling(); // bring to front
    }

    public void Close()
    {
        windowRoot.SetActive(false);
    }
}
