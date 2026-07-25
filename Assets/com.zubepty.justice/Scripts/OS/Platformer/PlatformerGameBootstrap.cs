using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class PlatformerGameBootstrap
{
    private const string SceneName = "Nuke_Me";
    private const string WindowName = "Platformer-Game";
    private const string NextGameWindowName = "Home - Shuffle";
    private const string ContainerName = "Container";
    private const string RuntimeRootName = "PlatformerRuntimeRoot";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void BuildPlatformer()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        if (activeScene.name != SceneName)
        {
            return;
        }

        RectTransform window = FindRectTransform(activeScene, WindowName);

        if (window == null)
        {
            return;
        }

        RectTransform nextGameWindow = FindRectTransform(activeScene, NextGameWindowName);

        RectTransform container = FindDirectChild(window, ContainerName);

        if (container == null || FindDirectChild(container, RuntimeRootName) != null)
        {
            return;
        }

        RectTransform root = CreatePanel(RuntimeRootName, container, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, new Color(0.04f, 0.045f, 0.055f, 1f));

        Vector2 centerAnchor = new Vector2(0.5f, 0.5f);
        RectTransform floorLeft = CreateBlock("FloorLeft", root, centerAnchor, centerAnchor, new Vector2(-310f, -220f), new Vector2(320f, 34f), new Color(0.31f, 0.31f, 0.34f, 1f));
        RectTransform trapFloor = CreateBlock("CollapsingFloor", root, centerAnchor, centerAnchor, new Vector2(-55f, -220f), new Vector2(190f, 34f), new Color(0.31f, 0.31f, 0.34f, 1f));
        RectTransform floorRight = CreateBlock("FloorRight", root, centerAnchor, centerAnchor, new Vector2(275f, -220f), new Vector2(380f, 34f), new Color(0.31f, 0.31f, 0.34f, 1f));
        RectTransform ledgeOne = CreateBlock("MiddlePlatform", root, centerAnchor, centerAnchor, new Vector2(-120f, -85f), new Vector2(190f, 24f), new Color(0.24f, 0.26f, 0.31f, 1f));
        RectTransform ledgeTwo = CreateBlock("RightPlatform", root, centerAnchor, centerAnchor, new Vector2(185f, 35f), new Vector2(210f, 24f), new Color(0.24f, 0.26f, 0.31f, 1f));

        RectTransform goal = CreateBlock("Goal", root, centerAnchor, centerAnchor, new Vector2(465f, -151f), new Vector2(34f, 104f), new Color(0.08f, 0.78f, 0.36f, 1f));
        RectTransform player = CreateBlock("PlayerCube", root, centerAnchor, centerAnchor, new Vector2(-415f, -182f), new Vector2(42f, 42f), new Color(0.25f, 0.62f, 1f, 1f));

        TextMeshProUGUI helpText = CreateText("HelpText", root, "A/D or Arrow Keys: move    Space/W/Up: jump", TextAlignmentOptions.Left, 18f);
        RectTransform helpRect = helpText.rectTransform;
        helpRect.anchorMin = new Vector2(0f, 1f);
        helpRect.anchorMax = new Vector2(1f, 1f);
        helpRect.pivot = new Vector2(0.5f, 1f);
        helpRect.anchoredPosition = new Vector2(0f, -16f);
        helpRect.sizeDelta = new Vector2(-32f, 32f);

        TextMeshProUGUI statusText = CreateText("StatusText", root, "My house is on the right, just go there and get licencse", TextAlignmentOptions.Center, 18f);
        RectTransform statusRect = statusText.rectTransform;
        statusRect.anchorMin = new Vector2(0f, 1f);
        statusRect.anchorMax = new Vector2(1f, 1f);
        statusRect.pivot = new Vector2(0.5f, 1f);
        statusRect.anchoredPosition = new Vector2(0f, -55f);
        statusRect.sizeDelta = new Vector2(-32f, 48f);

        PlatformerGameController controller = root.gameObject.AddComponent<PlatformerGameController>();
        controller.Configure(player, goal, statusText, trapFloor, window.gameObject, nextGameWindow != null ? nextGameWindow.gameObject : null, floorLeft, floorRight, ledgeOne, ledgeTwo);
    }

    private static RectTransform FindRectTransform(Scene scene, string objectName)
    {
        RectTransform[] transforms = Resources.FindObjectsOfTypeAll<RectTransform>();

        foreach (RectTransform rectTransform in transforms)
        {
            if (rectTransform.gameObject.scene == scene && rectTransform.name == objectName)
            {
                return rectTransform;
            }
        }

        return null;
    }

    private static RectTransform FindDirectChild(RectTransform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName && child is RectTransform rectTransform)
            {
                return rectTransform;
            }
        }

        return null;
    }

    private static RectTransform CreatePanel(string objectName, RectTransform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 sizeDelta, Color color)
    {
        RectTransform rectTransform = CreateRect(objectName, parent, anchorMin, anchorMax, anchoredPosition, sizeDelta);
        Image image = rectTransform.gameObject.AddComponent<Image>();
        image.color = color;
        image.raycastTarget = false;
        return rectTransform;
    }

    private static RectTransform CreateBlock(string objectName, RectTransform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 sizeDelta, Color color)
    {
        RectTransform rectTransform = CreateRect(objectName, parent, anchorMin, anchorMax, anchoredPosition, sizeDelta);
        Image image = rectTransform.gameObject.AddComponent<Image>();
        image.color = color;
        image.raycastTarget = false;
        return rectTransform;
    }

    private static RectTransform CreateRect(string objectName, RectTransform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 sizeDelta)
    {
        GameObject gameObject = new GameObject(objectName, typeof(RectTransform), typeof(CanvasRenderer));
        gameObject.layer = parent.gameObject.layer;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.SetParent(parent, false);
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = sizeDelta;
        return rectTransform;
    }

    private static TextMeshProUGUI CreateText(string objectName, RectTransform parent, string text, TextAlignmentOptions alignment, float fontSize)
    {
        GameObject gameObject = new GameObject(objectName, typeof(RectTransform), typeof(CanvasRenderer));
        gameObject.layer = parent.gameObject.layer;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.SetParent(parent, false);

        TextMeshProUGUI textComponent = gameObject.AddComponent<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.alignment = alignment;
        textComponent.fontSize = fontSize;
        textComponent.color = new Color(0.92f, 0.92f, 0.88f, 1f);
        textComponent.raycastTarget = false;

        return textComponent;
    }
}
