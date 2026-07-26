using UnityEngine;
using UnityEngine.SceneManagement;
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

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowImage(Sprite sprite)
    {
        fullImage.sprite = sprite;
        fullImage.preserveAspect = true;
        windowRoot.SetActive(true);
        windowRoot.transform.SetAsLastSibling(); // bring to front
    }

    public void Close()
    {
        windowRoot.SetActive(false);
    }
}
