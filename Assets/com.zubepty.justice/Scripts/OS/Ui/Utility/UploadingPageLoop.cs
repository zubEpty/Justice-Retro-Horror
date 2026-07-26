using DG.Tweening;
using UnityEngine;

public class UploadingPageLoop : MonoBehaviour
{
    [SerializeField] private RectTransform page;
    [SerializeField] private RectTransform pointA;
    [SerializeField] private RectTransform pointB;
    [SerializeField] private float moveDuration = 1.2f;

    private Tween pageTween;

    private void OnEnable()
    {
        CacheReferences();
        StartLoop();
    }

    private void OnDisable()
    {
        pageTween?.Kill();
        pageTween = null;
    }

    private void StartLoop()
    {
        if (page == null || pointA == null || pointB == null)
        {
            Debug.LogWarning("Uploading page loop is missing page, PointA, or PointB reference.", this);
            return;
        }

        pageTween?.Kill();
        page.anchoredPosition = pointA.anchoredPosition;

        pageTween = page
            .DOAnchorPos(pointB.anchoredPosition, moveDuration)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    private void CacheReferences()
    {
        if (page == null)
            page = FindChildRect("page");

        if (pointA == null)
            pointA = FindChildRect("PointA");

        if (pointB == null)
            pointB = FindChildRect("PointB");
    }

    private RectTransform FindChildRect(string objectName)
    {
        RectTransform[] children = GetComponentsInChildren<RectTransform>(true);

        foreach (RectTransform child in children)
        {
            if (child.name == objectName)
                return child;
        }

        return null;
    }
}
