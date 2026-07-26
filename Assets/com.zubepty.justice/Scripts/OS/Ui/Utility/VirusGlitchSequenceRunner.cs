using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VirusGlitchSequenceRunner : MonoBehaviour
{
    private const string SceneName = "Nuke_Me";
    private const string RunnerName = "VirusGlitchSequenceRunner";

    private static bool hasPlayed;

    private readonly List<GameObject> spawnedObjects = new List<GameObject>();

    private RectTransform screenRoot;
    private Vector3 originalRootPosition;

    public static void PlayOnce(GameObject openedWindow)
    {
        if (hasPlayed || openedWindow == null || SceneManager.GetActiveScene().name != SceneName)
            return;

        Canvas canvas = openedWindow.GetComponentInParent<Canvas>();
        if (canvas == null)
            canvas = FindSceneComponent<Canvas>();

        if (canvas == null)
            return;

        hasPlayed = true;

        GameObject runnerObject = new GameObject(RunnerName);
        runnerObject.transform.SetParent(canvas.transform, false);

        VirusGlitchSequenceRunner runner = runnerObject.AddComponent<VirusGlitchSequenceRunner>();
        runner.Begin(canvas.GetComponent<RectTransform>());
    }

    private void Begin(RectTransform canvasRect)
    {
        screenRoot = canvasRect;
        if (screenRoot != null)
            originalRootPosition = screenRoot.localPosition;

        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        CanvasGroup overlayGroup = CreateGlitchOverlay();

        yield return StartCoroutine(ShakeScreen(0.35f, 18f));
        yield return StartCoroutine(FlickerOverlay(overlayGroup, 0.7f, 0.08f, 0.34f));

        string[][] commandSets =
        {
            new[]
            {
                "> begula_payload.exe /silent",
                "Scanning desktop shortcuts...",
                "Found: Btn_Virus",
                "Privilege check: FAILED",
                "Retrying with stolen token..."
            },
            new[]
            {
                "> net session /ghost",
                "Opening hidden shell...",
                "Uploading local index...",
                "User hesitation detected."
            },
            new[]
            {
                "> format regret:",
                "Command rejected by antivirus.",
                "Disabling antivirus UI...",
                "Status: TOO LATE"
            },
            new[]
            {
                "> taskkill /f /im peace.exe",
                "Spawning recovery blocker...",
                "Keyboard automation: ON",
                "Keyboard automation: OFF"
            }
        };

        for (int i = 0; i < commandSets.Length; i++)
        {
            RectTransform commandWindow = CreateCommandWindow(i);
            yield return StartCoroutine(TypeLines(commandWindow, commandSets[i]));

            if (i == 1)
                StartCoroutine(ShakeScreen(0.45f, 13f));

            yield return new WaitForSeconds(Random.Range(0.08f, 0.22f));
        }

        yield return StartCoroutine(ShowBlueScreenFlash());
        yield return StartCoroutine(FlickerOverlay(overlayGroup, 1.2f, 0.04f, 0.2f));
        yield return new WaitForSeconds(0.35f);
        Cleanup();
    }

    private CanvasGroup CreateGlitchOverlay()
    {
        GameObject overlay = new GameObject("Virus_Glitch_Overlay", typeof(RectTransform), typeof(CanvasRenderer));
        overlay.transform.SetParent(transform.parent, false);
        overlay.transform.SetAsLastSibling();
        spawnedObjects.Add(overlay);

        RectTransform rect = overlay.GetComponent<RectTransform>();
        StretchToParent(rect);

        Image background = overlay.AddComponent<Image>();
        background.color = new Color(0f, 0.02f, 0.01f, 0.08f);
        background.raycastTarget = false;

        CanvasGroup group = overlay.AddComponent<CanvasGroup>();
        group.blocksRaycasts = false;
        group.alpha = 0f;

        for (int i = 0; i < 18; i++)
            CreateGlitchBand(rect, i);

        return group;
    }

    private void CreateGlitchBand(RectTransform overlayRect, int index)
    {
        GameObject band = new GameObject("Glitch_Band", typeof(RectTransform), typeof(CanvasRenderer));
        band.transform.SetParent(overlayRect, false);

        RectTransform rect = band.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, Random.Range(0.02f, 0.96f));
        rect.anchorMax = new Vector2(1f, rect.anchorMin.y);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(0f, Random.Range(2f, 15f));
        rect.anchoredPosition = new Vector2(Random.Range(-40f, 40f), 0f);

        Image image = band.AddComponent<Image>();
        image.raycastTarget = false;
        image.color = index % 3 == 0
            ? new Color(0.1f, 1f, 0.55f, Random.Range(0.1f, 0.28f))
            : new Color(1f, 0.04f, 0.04f, Random.Range(0.05f, 0.18f));
    }

    private RectTransform CreateCommandWindow(int index)
    {
        GameObject window = new GameObject("Auto_CMD_Window", typeof(RectTransform), typeof(CanvasRenderer));
        window.transform.SetParent(transform.parent, false);
        window.transform.SetAsLastSibling();
        spawnedObjects.Add(window);

        RectTransform rect = window.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(520f, 245f);
        rect.anchoredPosition = GetCommandPosition(index);

        Image frame = window.AddComponent<Image>();
        frame.color = new Color(0.005f, 0.005f, 0.006f, 0.96f);

        UnityEngine.UI.Outline outline = window.AddComponent<UnityEngine.UI.Outline>();
        outline.effectColor = new Color(0.15f, 1f, 0.45f, 0.65f);
        outline.effectDistance = new Vector2(1f, -1f);

        CreateText("Title", rect, "C:\\Windows\\System32\\cmd.exe", 17f, new Color(0.9f, 0.9f, 0.9f, 1f), TextAlignmentOptions.Left, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, 1f), new Vector2(12f, -8f), new Vector2(-24f, 26f));
        CreateText("Body", rect, "", 18f, new Color(0.45f, 1f, 0.58f, 1f), TextAlignmentOptions.TopLeft, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0f, 1f), new Vector2(15f, -43f), new Vector2(-30f, -58f));

        window.transform.localScale = Vector3.one * Random.Range(0.92f, 1.08f);
        return rect;
    }

    private Vector2 GetCommandPosition(int index)
    {
        Vector2[] positions =
        {
            new Vector2(-320f, 150f),
            new Vector2(250f, 95f),
            new Vector2(-120f, -115f),
            new Vector2(360f, -185f)
        };

        return positions[index % positions.Length] + new Vector2(Random.Range(-35f, 35f), Random.Range(-25f, 25f));
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

    private IEnumerator TypeLines(RectTransform commandWindow, string[] lines)
    {
        if (commandWindow == null)
            yield break;

        TextMeshProUGUI body = FindBodyText(commandWindow);
        if (body == null)
            yield break;

        body.text = "";

        for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
        {
            string line = lines[lineIndex];

            for (int charIndex = 0; charIndex < line.Length; charIndex++)
            {
                body.text += line[charIndex];
                yield return new WaitForSeconds(Random.Range(0.005f, 0.026f));
            }

            body.text += "\n";
            yield return new WaitForSeconds(Random.Range(0.05f, 0.16f));
        }
    }

    private IEnumerator ShakeScreen(float duration, float strength)
    {
        if (screenRoot == null)
            yield break;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float falloff = 1f - elapsed / duration;
            Vector2 offset = Random.insideUnitCircle * strength * falloff;
            screenRoot.localPosition = originalRootPosition + new Vector3(offset.x, offset.y, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        screenRoot.localPosition = originalRootPosition;
    }

    private IEnumerator FlickerOverlay(CanvasGroup group, float duration, float minAlpha, float maxAlpha)
    {
        if (group == null)
            yield break;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            group.alpha = Random.Range(minAlpha, maxAlpha);
            elapsed += Random.Range(0.025f, 0.075f);
            yield return new WaitForSeconds(Random.Range(0.025f, 0.075f));
        }

        group.alpha = 0f;
    }

    private IEnumerator ShowBlueScreenFlash()
    {
        GameObject blueScreen = new GameObject("Virus_BlueScreen_Flash", typeof(RectTransform), typeof(CanvasRenderer));
        blueScreen.transform.SetParent(transform.parent, false);
        blueScreen.transform.SetAsLastSibling();
        spawnedObjects.Add(blueScreen);

        RectTransform rect = blueScreen.GetComponent<RectTransform>();
        StretchToParent(rect);

        Image background = blueScreen.AddComponent<Image>();
        background.color = new Color(0.02f, 0.15f, 0.78f, 1f);
        background.raycastTarget = false;

        CanvasGroup group = blueScreen.AddComponent<CanvasGroup>();
        group.blocksRaycasts = false;
        group.alpha = 0f;

        CreateText("SadFace", rect, ":(", 96f, Color.white, TextAlignmentOptions.Left, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(96f, -86f), new Vector2(220f, 120f));
        CreateText("ErrorText", rect, "Your PC ran into a problem and needs to restart.\n\nbegula.exe caused a critical system error.\n\nCollecting error info: 94%\n\nStop code: PLAYER_OPENED_VIRUS_FILE", 34f, Color.white, TextAlignmentOptions.TopLeft, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0f, 1f), new Vector2(98f, -220f), new Vector2(-180f, -260f));

        yield return StartCoroutine(FadeCanvasGroup(group, 0f, 1f, 0.08f));
        yield return StartCoroutine(ShakeScreen(0.55f, 24f));
        yield return new WaitForSeconds(1.35f);
        yield return StartCoroutine(FadeCanvasGroup(group, 1f, 0f, 0.18f));

        spawnedObjects.Remove(blueScreen);
        Destroy(blueScreen);
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup group, float startAlpha, float endAlpha, float duration)
    {
        if (group == null)
            yield break;

        float elapsed = 0f;
        group.alpha = startAlpha;

        while (elapsed < duration)
        {
            float progress = duration <= 0f ? 1f : elapsed / duration;
            group.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
            elapsed += Time.deltaTime;
            yield return null;
        }

        group.alpha = endAlpha;
    }

    private void Cleanup()
    {
        if (screenRoot != null)
            screenRoot.localPosition = originalRootPosition;

        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (spawnedObjects[i] != null)
                Destroy(spawnedObjects[i]);
        }

        Destroy(gameObject);
    }

    private static TextMeshProUGUI FindBodyText(RectTransform commandWindow)
    {
        TextMeshProUGUI[] texts = commandWindow.GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (TextMeshProUGUI text in texts)
        {
            if (text.name == "Body")
                return text;
        }

        return texts.Length > 0 ? texts[texts.Length - 1] : null;
    }

    private static void StretchToParent(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.pivot = new Vector2(0.5f, 0.5f);
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
}
