using UnityEngine;
using UnityEngine.UI;

public class ImageThumbnail : MonoBehaviour
{
    public Image thumbnailImage;

    public void OnClick()
    {
        ImageViewer.Instance.ShowImage(thumbnailImage.sprite);
    }
}
