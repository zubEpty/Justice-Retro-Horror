using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BrowserLoadingUi : MonoBehaviour
{
    public static BrowserLoadingUi Instance;

    [Header("UI")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image progressFill;

    [Header("Timing")]
    [SerializeField] private Vector2 loadTimeRange = new Vector2(0.6f, 1.8f);

    private Tween _progressTween;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        canvasGroup.alpha = 0;
        canvasGroup.gameObject.SetActive(false);
    }

    public void Load(Action onComplete)
    {
        float loadTime = UnityEngine.Random.Range(
            loadTimeRange.x,
            loadTimeRange.y
        );

        canvasGroup.gameObject.SetActive(true);
        canvasGroup.alpha = 1;
        progressFill.fillAmount = 0;

        _progressTween?.Kill();

        _progressTween = progressFill
            .DOFillAmount(1f, loadTime)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                canvasGroup
                    .DOFade(0f, 0.2f)
                    .OnComplete(() =>
                    {
                        canvasGroup.gameObject.SetActive(false);
                        onComplete?.Invoke();
                    });
            });
    }
}
