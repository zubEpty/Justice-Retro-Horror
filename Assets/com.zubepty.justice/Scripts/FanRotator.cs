using UnityEngine;
using DG.Tweening;

public class FanRotator : MonoBehaviour
{
    public float rotationSpeed = 360f; // Degrees per second

    void Start()
    {
        // Set initial fixed rotation
        transform.rotation = Quaternion.Euler(-90, 0, 0);

        // Create infinite Y-axis rotation while preserving X and Z
        DOTween.To(() => transform.eulerAngles.y,
                   y => transform.rotation = Quaternion.Euler(-90, y, 0),
                   360,
                   1f)
               .SetEase(Ease.Linear)
               .SetLoops(-1, LoopType.Incremental);
    }
}
