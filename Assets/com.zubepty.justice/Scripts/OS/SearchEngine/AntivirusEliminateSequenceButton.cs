using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AntivirusEliminateSequenceButton : MonoBehaviour
{
    [SerializeField] private GameObject successPanel;
    [SerializeField] private GameObject threatCheckerPanel;
    [SerializeField] private GameObject virusButton;
    [SerializeField] private GameObject timerObject;
    [SerializeField] private float eliminateDuration = 3.2f;
    [SerializeField] private float successRevealDelay = 1.5f;

    private Button _button;
    private Coroutine _eliminateRoutine;
    private GameObject _eliminateWindow;
    private CanvasGroup _eliminateGroup;

    private readonly string[] _statusLines =
    {
        "Locking infected executable...",
        "Suspending payload heartbeat...",
        "Purging startup hooks...",
        "Scrubbing encrypted residue...",
        "Removing ransom timer...",
        "Restoring system control..."
    };

    private void Awake()
    {
        _button = GetComponent<Button>();
        ResolveReferences();
        _button.onClick.AddListener(StartEliminate);
    }

    private void OnDestroy()
    {
        if (_button != null)
            _button.onClick.RemoveListener(StartEliminate);
    }

    private void OnDisable()
    {
        if (_eliminateRoutine != null)
        {
            StopCoroutine(_eliminateRoutine);
            _eliminateRoutine = null;
        }

        if (_eliminateWindow != null)
            Destroy(_eliminateWindow);

        if (_button != null)
            _button.interactable = true;
    }

    private void StartEliminate()
    {
        if (_eliminateRoutine != null)
            return;

        ResolveReferences();
        _eliminateRoutine = StartCoroutine(PlayEliminateSequence());
    }

    private IEnumerator PlayEliminateSequence()
    {
        if (_button != null)
            _button.interactable = false;

        EliminateUi ui = CreateEliminateWindow();

        if (_eliminateGroup != null)
            _eliminateGroup.DOFade(1f, 0.18f).SetEase(Ease.OutQuad);

        if (_eliminateWindow != null)
        {
            _eliminateWindow.transform.localScale = Vector3.one * 0.92f;
            _eliminateWindow.transform.DOScale(1f, 0.28f).SetEase(Ease.OutBack);
        }

        Tween progressTween = null;
        if (ui.ProgressFill != null)
        {
            ui.ProgressFill.fillAmount = 0f;
            progressTween = ui.ProgressFill.DOFillAmount(1f, eliminateDuration).SetEase(Ease.InOutSine);
        }

        yield return StartCoroutine(PlayStatusLines(ui));

        if (progressTween == null)
            yield return new WaitForSeconds(0.2f);
        else
            yield return progressTween.WaitForCompletion();

        if (ui.StatusText != null)
            ui.StatusText.text = "Threat eliminated. System recovered.";

        if (ui.ProgressLabel != null)
            ui.ProgressLabel.text = "100%";

        yield return new WaitForSeconds(successRevealDelay);

        DisableVirusButton();
        StopAndRemoveTimer();

        if (_eliminateGroup != null)
            yield return _eliminateGroup.DOFade(0f, 0.18f).WaitForCompletion();

        if (_eliminateWindow != null)
            Destroy(_eliminateWindow);

        if (successPanel != null)
        {
            successPanel.SetActive(true);
            successPanel.transform.SetAsLastSibling();
        }

        if (_button != null)
            _button.interactable = true;

        _eliminateRoutine = null;

        if (threatCheckerPanel != null)
            threatCheckerPanel.SetActive(false);
    }

    private IEnumerator PlayStatusLines(EliminateUi ui)
    {
        float lineDelay = eliminateDuration / Mathf.Max(1, _statusLines.Length);

        for (int i = 0; i < _statusLines.Length; i++)
        {
            int percent = Mathf.RoundToInt((i + 1f) / _statusLines.Length * 100f);

            if (ui.StatusText != null)
                ui.StatusText.text = _statusLines[i];

            if (ui.ProgressLabel != null)
                ui.ProgressLabel.text = $"{percent}%";

            if (ui.QuarantineText != null)
                ui.QuarantineText.text += $"<color=#6CFF8D>> {_statusLines[i]}</color>\n";

            if (ui.PulseNode != null)
            {
                ui.PulseNode.DOKill();
                ui.PulseNode.localScale = Vector3.one;
                ui.PulseNode.DOScale(1.25f, 0.16f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad);
            }

            yield return new WaitForSeconds(lineDelay);
        }
    }

    private EliminateUi CreateEliminateWindow()
    {
        Transform parent = threatCheckerPanel != null && threatCheckerPanel.transform.parent != null
            ? threatCheckerPanel.transform.parent
            : transform.parent;

        _eliminateWindow = new GameObject("Begula_Eliminate_UI", typeof(RectTransform), typeof(CanvasRenderer));
        _eliminateWindow.transform.SetParent(parent, false);
        _eliminateWindow.transform.SetAsLastSibling();

        RectTransform rect = _eliminateWindow.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(4f, 0f);
        rect.sizeDelta = new Vector2(647f, 372f);

        Image background = _eliminateWindow.AddComponent<Image>();
        background.color = new Color(0.015f, 0.015f, 0.018f, 0.98f);

        UnityEngine.UI.Outline outline = _eliminateWindow.AddComponent<UnityEngine.UI.Outline>();
        outline.effectColor = new Color(0.15f, 1f, 0.38f, 0.85f);
        outline.effectDistance = new Vector2(2f, -2f);

        _eliminateGroup = _eliminateWindow.AddComponent<CanvasGroup>();
        _eliminateGroup.alpha = 0f;
        _eliminateGroup.blocksRaycasts = true;

        CreateText("Title", rect, "ELIMINATING ACTIVE THREATS", 28f, Color.white, TextAlignmentOptions.Left,
            new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, 1f), new Vector2(24f, -20f), new Vector2(-48f, 46f));

        TextMeshProUGUI status = CreateText("Status", rect, "Preparing removal engine...", 22f, new Color(0.75f, 1f, 0.78f, 1f), TextAlignmentOptions.Left,
            new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, 1f), new Vector2(24f, -74f), new Vector2(-48f, 38f));

        Image progressBackground = CreateImage("Progress_Background", rect, new Color(0.04f, 0.12f, 0.07f, 1f),
            new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, 1f), new Vector2(24f, -122f), new Vector2(-92f, 28f));

        Image progressFill = CreateImage("Progress_Fill", progressBackground.rectTransform, new Color(0.16f, 1f, 0.34f, 1f),
            Vector2.zero, Vector2.one, new Vector2(0f, 0.5f), Vector2.zero, Vector2.zero);
        progressFill.type = Image.Type.Filled;
        progressFill.fillMethod = Image.FillMethod.Horizontal;
        progressFill.fillOrigin = 0;
        progressFill.fillAmount = 0f;

        TextMeshProUGUI progressLabel = CreateText("Progress_Label", rect, "0%", 20f, Color.white, TextAlignmentOptions.Right,
            new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-24f, -123f), new Vector2(60f, 30f));

        TextMeshProUGUI quarantine = CreateText("Quarantine_Log", rect, "", 18f, new Color(0.6f, 1f, 0.72f, 1f), TextAlignmentOptions.TopLeft,
            Vector2.zero, Vector2.one, new Vector2(0f, 1f), new Vector2(24f, -172f), new Vector2(-48f, -194f));

        Image pulseNode = CreateImage("Pulse_Node", rect, new Color(1f, 0.15f, 0.12f, 1f),
            new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f), new Vector2(-48f, -39f), new Vector2(24f, 24f));
        pulseNode.rectTransform.DOScale(1.18f, 0.4f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

        return new EliminateUi(status, quarantine, progressFill, progressLabel, pulseNode.rectTransform);
    }

    private TextMeshProUGUI CreateText(string objectName, RectTransform parent, string text, float fontSize, Color color, TextAlignmentOptions alignment, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 position, Vector2 sizeDelta)
    {
        GameObject textObject = new GameObject(objectName, typeof(RectTransform), typeof(CanvasRenderer));
        textObject.transform.SetParent(parent, false);

        RectTransform rect = textObject.GetComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = pivot;
        rect.anchoredPosition = position;
        rect.sizeDelta = sizeDelta;

        TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.fontSize = fontSize;
        textComponent.color = color;
        textComponent.alignment = alignment;
        textComponent.enableWordWrapping = false;
        textComponent.raycastTarget = false;

        return textComponent;
    }

    private Image CreateImage(string objectName, RectTransform parent, Color color, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 position, Vector2 sizeDelta)
    {
        GameObject imageObject = new GameObject(objectName, typeof(RectTransform), typeof(CanvasRenderer));
        imageObject.transform.SetParent(parent, false);

        RectTransform rect = imageObject.GetComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = pivot;
        rect.anchoredPosition = position;
        rect.sizeDelta = sizeDelta;
        rect.offsetMin = anchorMin == Vector2.zero && anchorMax == Vector2.one ? Vector2.zero : rect.offsetMin;
        rect.offsetMax = anchorMin == Vector2.zero && anchorMax == Vector2.one ? Vector2.zero : rect.offsetMax;

        Image image = imageObject.AddComponent<Image>();
        image.color = color;
        return image;
    }

    private void DisableVirusButton()
    {
        if (virusButton == null)
            virusButton = FindSceneObject("Btn_Virus");

        if (virusButton != null)
            virusButton.SetActive(false);
    }

    private void StopAndRemoveTimer()
    {
        RansomwareTimerRunner runner = FindSceneComponent<RansomwareTimerRunner>();
        if (runner != null)
        {
            runner.StopAllCoroutines();
            Destroy(runner.gameObject);
        }

        if (timerObject == null)
            timerObject = FindSceneObject("Timer");

        if (timerObject != null)
        {
            timerObject.transform.DOKill(true);
            Destroy(timerObject);
        }
    }

    private void ResolveReferences()
    {
        if (successPanel == null)
            successPanel = FindSceneObject("Game_Success_Panel");

        if (threatCheckerPanel == null)
            threatCheckerPanel = FindSceneObject("ThreatChecker");

        if (virusButton == null)
            virusButton = FindSceneObject("Btn_Virus");

        if (timerObject == null)
            timerObject = FindSceneObject("Timer");
    }

    private static T FindSceneComponent<T>() where T : Component
    {
        T[] components = Resources.FindObjectsOfTypeAll<T>();
        foreach (T component in components)
        {
            if (component.gameObject.scene.IsValid())
                return component;
        }

        return null;
    }

    private static GameObject FindSceneObject(string objectName)
    {
        GameObject[] sceneObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject sceneObject in sceneObjects)
        {
            if (sceneObject.name == objectName && sceneObject.scene.IsValid())
                return sceneObject;
        }

        return null;
    }

    private readonly struct EliminateUi
    {
        public EliminateUi(TextMeshProUGUI statusText, TextMeshProUGUI quarantineText, Image progressFill, TextMeshProUGUI progressLabel, RectTransform pulseNode)
        {
            StatusText = statusText;
            QuarantineText = quarantineText;
            ProgressFill = progressFill;
            ProgressLabel = progressLabel;
            PulseNode = pulseNode;
        }

        public TextMeshProUGUI StatusText { get; }
        public TextMeshProUGUI QuarantineText { get; }
        public Image ProgressFill { get; }
        public TextMeshProUGUI ProgressLabel { get; }
        public RectTransform PulseNode { get; }
    }
}
