using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AntivirusScanSequenceButton : MonoBehaviour
{
    [SerializeField] private GameObject threatCheckerPanel;
    [SerializeField] private float lineDelay = 0.28f;
    [SerializeField] private float completePause = 0.55f;
    [SerializeField] private bool useBlueConsole = true;

    private Button _button;
    private Coroutine _scanRoutine;
    private GameObject _consoleWindow;

    private readonly string[] _scanLines =
    {
        "BEGULA AntiVIRUS v0.9.1",
        "Initializing threat scanner...",
        "Checking memory sectors...",
        "Scanning system32\\drivers...",
        "Scanning browser cache...",
        "Suspicious payload signature found.",
        "Verifying infection chain...",
        "Threat scan complete."
    };

    private void Awake()
    {
        _button = GetComponent<Button>();
        ResolveReferences();
        _button.onClick.AddListener(StartScan);
    }

    private void OnDestroy()
    {
        if (_button != null)
            _button.onClick.RemoveListener(StartScan);
    }

    private void OnDisable()
    {
        if (_scanRoutine != null)
        {
            StopCoroutine(_scanRoutine);
            _scanRoutine = null;
        }

        if (_consoleWindow != null)
            Destroy(_consoleWindow);

        if (_button != null)
            _button.interactable = true;
    }

    private void StartScan()
    {
        if (_scanRoutine != null)
            return;

        ResolveReferences();

        if (threatCheckerPanel != null)
            threatCheckerPanel.SetActive(false);

        _scanRoutine = StartCoroutine(PlayScanSequence());
    }

    private IEnumerator PlayScanSequence()
    {
        if (_button != null)
            _button.interactable = false;

        TextMeshProUGUI bodyText = CreateConsoleWindow();
        string output = "";

        for (int i = 0; i < _scanLines.Length; i++)
        {
            int percent = Mathf.RoundToInt((i + 1f) / _scanLines.Length * 100f);
            output += $"[{percent:000}%] {_scanLines[i]}\n";

            if (bodyText != null)
                bodyText.text = output + BuildProgressBar(percent);

            yield return new WaitForSeconds(lineDelay);
        }

        yield return new WaitForSeconds(completePause);

        if (_consoleWindow != null)
            Destroy(_consoleWindow);

        if (threatCheckerPanel != null)
        {
            threatCheckerPanel.SetActive(true);
            threatCheckerPanel.transform.SetAsLastSibling();
        }

        if (_button != null)
            _button.interactable = true;

        _scanRoutine = null;
    }

    private TextMeshProUGUI CreateConsoleWindow()
    {
        Transform parent = threatCheckerPanel != null && threatCheckerPanel.transform.parent != null
            ? threatCheckerPanel.transform.parent
            : transform.parent;

        _consoleWindow = new GameObject("Begula_Scan_Console", typeof(RectTransform), typeof(CanvasRenderer));
        _consoleWindow.transform.SetParent(parent, false);
        _consoleWindow.transform.SetAsLastSibling();

        RectTransform rect = _consoleWindow.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(4f, 0f);
        rect.sizeDelta = new Vector2(647f, 372f);

        Image background = _consoleWindow.AddComponent<Image>();
        background.color = useBlueConsole
            ? new Color(0.01f, 0.07f, 0.42f, 0.98f)
            : new Color(0.006f, 0.006f, 0.008f, 0.98f);

        UnityEngine.UI.Outline outline = _consoleWindow.AddComponent<UnityEngine.UI.Outline>();
        outline.effectColor = useBlueConsole
            ? new Color(0.32f, 0.72f, 1f, 0.9f)
            : new Color(0.15f, 1f, 0.45f, 0.8f);
        outline.effectDistance = new Vector2(2f, -2f);

        CreateText("Title", rect, "BEGULA AntiVIRUS Threat Console", 26f, Color.white, TextAlignmentOptions.Left,
            new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, 1f), new Vector2(22f, -18f), new Vector2(-44f, 44f));

        return CreateText("Body", rect, "", 21f, new Color(0.55f, 1f, 0.78f, 1f), TextAlignmentOptions.TopLeft,
            Vector2.zero, Vector2.one, new Vector2(0f, 1f), new Vector2(24f, -72f), new Vector2(-48f, -96f));
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

    private string BuildProgressBar(int percent)
    {
        const int width = 24;
        int filled = Mathf.RoundToInt(percent / 100f * width);
        return "\n[" + new string('#', filled) + new string('.', width - filled) + $"] {percent}%";
    }

    private void ResolveReferences()
    {
        if (threatCheckerPanel == null)
            threatCheckerPanel = FindSceneObject("ThreatChecker");
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
}
