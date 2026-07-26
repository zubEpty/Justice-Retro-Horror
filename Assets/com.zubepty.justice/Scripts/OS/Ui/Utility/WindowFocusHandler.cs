using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowFocusHandler : MonoBehaviour, IPointerDownHandler
{
    [Header("Alert")]
    [SerializeField] private bool isAlert;

    [Header("Ransomware Timer")]
    [SerializeField] private float ransomwareTimerDelay = 1f;
    [SerializeField] private float ransomwareTimerDurationSeconds = 300f;
    [SerializeField] private float ransomwareTimerSlideDistance = 520f;
    [SerializeField] private float ransomwareTimerSlideDuration = 0.45f;

    [Header("Virus Glitch Audio")]
    [SerializeField] private AudioClip virusScratchClip;
    [SerializeField] private AudioClip virusCommandErrorClip;
    [SerializeField] private AudioClip virusBlueScreenClip;

    private bool hasRansomwareTimerHiddenPosition;
    private Vector2 ransomwareTimerHiddenPosition;
    private static RansomwareTimerRunner ransomwareTimerRunner;

    private bool IsAlertWindow => isAlert || gameObject.name.StartsWith("Alert Panel") || gameObject.name.Trim() == "Password_Alert_Panel";

    private void OnEnable()
    {
        if (IsAlertWindow)
            transform.SetAsLastSibling();
    }

    public void OpenWindow(GameObject window)
    {
        if (window == null)
            return;

        window.SetActive(true);
        window.transform.SetAsLastSibling();
        BringActiveAlertsToFront();

        if (window.name == "Ransomware_Window")
        {
            StartRansomwareTimer();
            VirusGlitchSequenceRunner.PlayOnce(window, virusScratchClip, virusCommandErrorClip, virusBlueScreenClip);
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        BringActiveAlertsToFront();
    }

    private void StartRansomwareTimer()
    {
        RansomwareTimerRunner timerRunner = GetRansomwareTimerRunner();
        timerRunner.StartTimer(this, ransomwareTimerDelay, ransomwareTimerDurationSeconds, ransomwareTimerSlideDistance, ransomwareTimerSlideDuration);
    }

    private static RansomwareTimerRunner GetRansomwareTimerRunner()
    {
        if (ransomwareTimerRunner != null)
            return ransomwareTimerRunner;

        GameObject runnerObject = new GameObject("RansomwareTimerRunner");
        ransomwareTimerRunner = runnerObject.AddComponent<RansomwareTimerRunner>();
        return ransomwareTimerRunner;
    }

    internal IEnumerator RansomwareTimerSequence(float timerDelay, float timerDurationSeconds, float timerSlideDistance, float timerSlideDuration)
    {
        GameObject timerObject = FindSceneGameObject("Timer");
        GameObject gameOverPanel = FindSceneGameObject("Gameover_panel");

        if (timerObject == null)
        {
            Debug.LogWarning("Ransomware timer could not find a GameObject named 'Timer'.", this);
            yield break;
        }

        TextMeshProUGUI timerText = FindTimerText(timerObject);
        RectTransform timerRect = timerObject.GetComponent<RectTransform>();

        if (timerRect != null)
        {
            if (!hasRansomwareTimerHiddenPosition)
            {
                ransomwareTimerHiddenPosition = timerRect.anchoredPosition;
                hasRansomwareTimerHiddenPosition = true;
            }

            timerRect.anchoredPosition = ransomwareTimerHiddenPosition;
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (timerText != null)
            timerText.text = FormatTime(timerDurationSeconds);

        yield return new WaitForSeconds(timerDelay);

        timerObject.SetActive(true);
        timerObject.transform.SetAsLastSibling();
        BringActiveAlertsToFront();

        if (timerRect != null)
            yield return SlideTimerIntoView(timerRect, timerSlideDistance, timerSlideDuration);

        float remainingSeconds = timerDurationSeconds;

        while (remainingSeconds > 0f)
        {
            if (timerText != null)
                timerText.text = FormatTime(remainingSeconds);

            remainingSeconds -= Time.deltaTime;
            yield return null;
        }

        if (timerText != null)
            timerText.text = "00:00";

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            gameOverPanel.transform.SetAsLastSibling();
        }

        BringActiveAlertsToFront();
    }

    private IEnumerator SlideTimerIntoView(RectTransform timerRect, float timerSlideDistance, float timerSlideDuration)
    {
        Vector2 startPosition = timerRect.anchoredPosition;
        Vector2 endPosition = startPosition + Vector2.left * timerSlideDistance;
        float elapsed = 0f;

        while (elapsed < timerSlideDuration)
        {
            float progress = elapsed / timerSlideDuration;
            progress = Mathf.SmoothStep(0f, 1f, progress);
            timerRect.anchoredPosition = Vector2.LerpUnclamped(startPosition, endPosition, progress);
            elapsed += Time.deltaTime;
            yield return null;
        }

        timerRect.anchoredPosition = endPosition;
    }

    private TextMeshProUGUI FindTimerText(GameObject timerObject)
    {
        TextMeshProUGUI[] texts = timerObject.GetComponentsInChildren<TextMeshProUGUI>(true);

        foreach (TextMeshProUGUI text in texts)
        {
            if (text.gameObject.name == "Time")
                return text;
        }

        return texts.Length > 0 ? texts[0] : null;
    }

    private string FormatTime(float remainingSeconds)
    {
        int totalSeconds = Mathf.Max(0, Mathf.CeilToInt(remainingSeconds) - 1);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        return $"{minutes:00}:{seconds:00}";
    }

    private static void BringActiveAlertsToFront()
    {
        WindowFocusHandler[] focusHandlers = Resources.FindObjectsOfTypeAll<WindowFocusHandler>();

        foreach (WindowFocusHandler focusHandler in focusHandlers)
        {
            if (focusHandler == null || !focusHandler.gameObject.activeInHierarchy || !focusHandler.IsAlertWindow)
                continue;

            focusHandler.transform.SetAsLastSibling();
        }
    }

    private static GameObject FindSceneGameObject(string objectName)
    {
        GameObject[] gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.name == objectName && gameObject.scene.IsValid())
                return gameObject;
        }

        return null;
    }
}
