using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CCTVUihandler : MonoBehaviour
{
    [SerializeField] private Image _dotImage;

    void Start()
    {
        StartBlinking();
    }

    void StartBlinking()
    {
        _dotImage.DOFade(0f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
