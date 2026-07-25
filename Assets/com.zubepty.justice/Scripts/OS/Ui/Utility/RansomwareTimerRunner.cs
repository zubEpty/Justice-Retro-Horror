using System.Collections;
using UnityEngine;

public sealed class RansomwareTimerRunner : MonoBehaviour
{
    private bool timerStarted;

    public void StartTimer(WindowFocusHandler timerOwner, float timerDelay, float timerDurationSeconds, float timerSlideDistance, float timerSlideDuration)
    {
        if (timerStarted || timerOwner == null)
            return;

        timerStarted = true;
        StartCoroutine(RunTimer(timerOwner, timerDelay, timerDurationSeconds, timerSlideDistance, timerSlideDuration));
    }

    private IEnumerator RunTimer(WindowFocusHandler timerOwner, float timerDelay, float timerDurationSeconds, float timerSlideDistance, float timerSlideDuration)
    {
        yield return timerOwner.RansomwareTimerSequence(timerDelay, timerDurationSeconds, timerSlideDistance, timerSlideDuration);
    }
}
