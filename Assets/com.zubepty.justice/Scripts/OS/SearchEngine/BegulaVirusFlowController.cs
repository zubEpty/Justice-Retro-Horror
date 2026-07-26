using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BegulaVirusFlowController : MonoBehaviour
{
    [SerializeField] private Button scanButton;
    [SerializeField] private GameObject licenseValidatorPanel;
    [SerializeField] private Button helpButton;
    [SerializeField] private GameObject searchEngineWindow;
    [SerializeField] private WindowFocusHandler searchEngineFocusHandler;
    [SerializeField] private SearchEngineManager searchEngineManager;
    [SerializeField] private GameObject antivirusHelpPage;
    [SerializeField] private string antivirusHelpUrl = "www.antivirus.com/help";
    [SerializeField] private Button hackersAddressButton;
    [SerializeField] private GameObject platformerGameWindow;
    [SerializeField] private WindowFocusHandler platformerFocusHandler;

    [Header("Project Submission")]
    [SerializeField] private Button submitProjectButton;
    [SerializeField] private TextMeshProUGUI submitProjectButtonText;
    [SerializeField] private GameObject submissionConfirmationPanel;
    [SerializeField] private Button submissionOkButton;
    [SerializeField] private GameObject uploadingPanel;
    [SerializeField] private Image uploadingProgressFill;
    [SerializeField] private float uploadDuration = 2.4f;

    [Header("Email Notification")]
    [SerializeField] private GameObject emailAlertPanel;
    [SerializeField] private RectTransform emailAlertRect;
    [SerializeField] private Button emailAlertOkButton;
    [SerializeField] private GameObject emailWindow;
    [SerializeField] private WindowFocusHandler emailFocusHandler;
    [SerializeField] private Button lindaEmailButton;
    [SerializeField] private GameObject emailContentPanel;
    [SerializeField] private Button emailDownloadButton;
    [SerializeField] private AntivirusDownloadController antivirusDownloadController;
    [SerializeField] private GameObject emailDownloadTarget;
    [SerializeField] private float emailAlertSlideDistance = 520f;
    [SerializeField] private float emailAlertSlideDuration = 0.45f;

    [Header("Beluga Ending")]
    [SerializeField] private Button gameSuccessOkButton;
    [SerializeField] private GameObject gameSuccessPanel;
    [SerializeField] private Image desktopBackground;
    [SerializeField] private Image[] appIconImages;
    [SerializeField] private Sprite belugaSprite;
    [SerializeField] private GameObject toolbar;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private AudioClip endingScratchClip;
    [SerializeField] private AudioClip endingCommandErrorClip;
    [SerializeField] private AudioClip endingBlueScreenClip;
    [SerializeField] private string endingWarningMessage = "This pc is now owned by Begula, Get out.";

    private Vector2 _emailAlertShownPosition;
    private Vector2 _emailAlertHiddenPosition;
    private bool _projectSubmitted;
    private bool _submissionUploadInProgress;
    private bool _submissionReferencesResolved;
    private bool _submissionListenersHooked;
    private bool _endingReferencesResolved;
    private bool _endingListenerHooked;
    private bool _belugaEndingStarted;
    private Tween _submissionUploadTween;

    public bool IsSubmittedEmailAvailable => _projectSubmitted;

    private void Awake()
    {
        ResolveSubmissionReferences();
        ResolveEndingReferences();

        if (scanButton != null)
            scanButton.onClick.AddListener(ShowLicenseValidator);

        if (helpButton != null)
            helpButton.onClick.AddListener(OpenAntivirusHelpPage);

        if (hackersAddressButton != null)
            hackersAddressButton.onClick.AddListener(OpenPlatformerGame);

        InitializeSubmissionState();
        HookEndingListener();
    }

    private void OnEnable()
    {
        HookSubmissionListeners();
        HookEndingListener();
    }

    private void OnDestroy()
    {
        if (scanButton != null)
            scanButton.onClick.RemoveListener(ShowLicenseValidator);

        if (helpButton != null)
            helpButton.onClick.RemoveListener(OpenAntivirusHelpPage);

        if (hackersAddressButton != null)
            hackersAddressButton.onClick.RemoveListener(OpenPlatformerGame);

        _submissionUploadTween?.Kill();
        UnhookSubmissionListeners();
        UnhookEndingListener();
    }

    public void ConfirmProjectSubmission()
    {
        ResolveSubmissionReferences();
        ConfirmSubmission();
    }

    public void StartProjectSubmission()
    {
        ResolveSubmissionReferences();
        ShowSubmissionConfirmation();
    }

    public void OpenSubmittedEmail()
    {
        ResolveSubmissionReferences();
        OpenEmailWindow();
    }

    public void StartBelugaEnding()
    {
        if (_belugaEndingStarted)
            return;

        _belugaEndingStarted = true;
        ResolveEndingReferences();

        if (gameSuccessOkButton != null)
            gameSuccessOkButton.interactable = false;

        CloseOpenDesktopWindows();
        ApplyBelugaDesktop();

        bool glitchStarted = VirusGlitchSequenceRunner.PlayAgain(gameObject, endingScratchClip, endingCommandErrorClip, endingBlueScreenClip, ShowBelugaWarning);
        if (!glitchStarted)
            ShowBelugaWarning();
    }

    private void HookSubmissionListeners()
    {
        ResolveSubmissionReferences();

        if (_submissionListenersHooked)
            return;

        if (submitProjectButton != null)
            submitProjectButton.onClick.RemoveListener(ShowSubmissionConfirmation);
        if (submitProjectButton != null)
            submitProjectButton.onClick.AddListener(ShowSubmissionConfirmation);

        if (submissionOkButton != null)
            submissionOkButton.onClick.RemoveListener(ConfirmSubmission);
        if (submissionOkButton != null)
            submissionOkButton.onClick.AddListener(ConfirmSubmission);

        if (emailAlertOkButton != null)
            emailAlertOkButton.onClick.RemoveListener(OpenEmailWindow);
        if (emailAlertOkButton != null)
            emailAlertOkButton.onClick.AddListener(OpenEmailWindow);

        if (lindaEmailButton != null)
            lindaEmailButton.onClick.RemoveListener(OpenLindaEmail);
        if (lindaEmailButton != null)
            lindaEmailButton.onClick.AddListener(OpenLindaEmail);

        if (emailDownloadButton != null)
            emailDownloadButton.onClick.RemoveListener(StartEmailDownload);
        if (emailDownloadButton != null)
            emailDownloadButton.onClick.AddListener(StartEmailDownload);

        _submissionListenersHooked = true;
    }

    private void UnhookSubmissionListeners()
    {
        if (submitProjectButton != null)
            submitProjectButton.onClick.RemoveListener(ShowSubmissionConfirmation);

        if (submissionOkButton != null)
            submissionOkButton.onClick.RemoveListener(ConfirmSubmission);

        if (emailAlertOkButton != null)
            emailAlertOkButton.onClick.RemoveListener(OpenEmailWindow);

        if (lindaEmailButton != null)
            lindaEmailButton.onClick.RemoveListener(OpenLindaEmail);

        if (emailDownloadButton != null)
            emailDownloadButton.onClick.RemoveListener(StartEmailDownload);

        _submissionListenersHooked = false;
    }

    private void HookEndingListener()
    {
        ResolveEndingReferences();

        if (_endingListenerHooked)
            return;

        if (gameSuccessOkButton == null)
            return;

        gameSuccessOkButton.onClick.RemoveListener(StartBelugaEnding);
        gameSuccessOkButton.onClick.AddListener(StartBelugaEnding);

        _endingListenerHooked = true;
    }

    private void UnhookEndingListener()
    {
        if (gameSuccessOkButton != null)
            gameSuccessOkButton.onClick.RemoveListener(StartBelugaEnding);

        _endingListenerHooked = false;
    }

    private void ShowLicenseValidator()
    {
        if (licenseValidatorPanel == null)
            return;

        licenseValidatorPanel.SetActive(true);
        licenseValidatorPanel.transform.SetAsLastSibling();
    }

    private void OpenAntivirusHelpPage()
    {
        if (licenseValidatorPanel != null)
            licenseValidatorPanel.SetActive(false);

        OpenWindow(searchEngineWindow, searchEngineFocusHandler);

        if (searchEngineManager != null)
            searchEngineManager.OpenBrowserPage(antivirusHelpPage, antivirusHelpUrl);
    }

    private void OpenPlatformerGame()
    {
        OpenWindow(platformerGameWindow, platformerFocusHandler);
    }

    private void ShowSubmissionConfirmation()
    {
        if (_projectSubmitted || submissionConfirmationPanel == null)
            return;

        if (_submissionUploadInProgress)
            return;

        if (uploadingPanel == null || uploadingProgressFill == null)
        {
            ShowSubmissionConfirmationPanel();
            return;
        }

        _submissionUploadInProgress = true;

        if (submitProjectButton != null)
            submitProjectButton.interactable = false;

        submissionConfirmationPanel.SetActive(false);

        uploadingPanel.SetActive(true);
        uploadingPanel.transform.SetAsLastSibling();

        uploadingProgressFill.DOKill();
        uploadingProgressFill.fillAmount = 0f;
        _submissionUploadTween?.Kill();
        _submissionUploadTween = uploadingProgressFill
            .DOFillAmount(1f, uploadDuration)
            .SetEase(Ease.Linear)
            .OnComplete(CompleteSubmissionUpload);
    }

    private void CompleteSubmissionUpload()
    {
        _submissionUploadInProgress = false;

        if (uploadingPanel != null)
            uploadingPanel.SetActive(false);

        ShowSubmissionConfirmationPanel();
    }

    private void ShowSubmissionConfirmationPanel()
    {
        if (_projectSubmitted || submissionConfirmationPanel == null)
            return;

        if (submitProjectButton != null)
            submitProjectButton.interactable = true;

        submissionConfirmationPanel.SetActive(true);
        submissionConfirmationPanel.transform.SetAsLastSibling();
    }

    private void ConfirmSubmission()
    {
        if (_projectSubmitted)
            return;

        _projectSubmitted = true;

        if (submissionConfirmationPanel != null)
            submissionConfirmationPanel.SetActive(false);

        if (submitProjectButtonText != null)
            submitProjectButtonText.text = "Submitted";

        if (submitProjectButton != null)
            submitProjectButton.interactable = false;

        ShowEmailAlert();
    }

    private void ShowEmailAlert()
    {
        if (emailAlertPanel == null)
            return;

        emailAlertPanel.SetActive(true);
        emailAlertPanel.transform.SetAsLastSibling();

        if (emailAlertRect == null)
            return;

        emailAlertRect.DOKill();
        emailAlertRect.anchoredPosition = _emailAlertHiddenPosition;
        emailAlertRect.DOAnchorPos(_emailAlertShownPosition, emailAlertSlideDuration).SetEase(Ease.OutCubic);
    }

    private void OpenEmailWindow()
    {
        if (emailAlertPanel != null)
            emailAlertPanel.SetActive(false);

        OpenWindow(emailWindow, emailFocusHandler);
        RaiseTransformChain(emailWindow);
        EnableLindaEmailButton();
    }

    private void EnableLindaEmailButton()
    {
        if (lindaEmailButton == null)
            return;

        SetParentsActive(lindaEmailButton.transform);
        lindaEmailButton.gameObject.SetActive(true);
        lindaEmailButton.transform.SetAsLastSibling();
        lindaEmailButton.transform.DOKill();
        lindaEmailButton.transform.localScale = Vector3.one * 0.88f;
        lindaEmailButton.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
        RaiseTransformChain(emailWindow);
    }

    private void OpenLindaEmail()
    {
        if (emailContentPanel == null)
            return;

        emailContentPanel.SetActive(true);
        emailContentPanel.transform.SetAsLastSibling();

        if (emailDownloadButton != null)
            emailDownloadButton.interactable = true;
    }

    private void StartEmailDownload()
    {
        if (antivirusDownloadController == null)
            return;

        antivirusDownloadController.StartDownload(emailDownloadTarget);
    }

    private void InitializeSubmissionState()
    {
        if (submissionConfirmationPanel != null)
            submissionConfirmationPanel.SetActive(false);

        if (uploadingPanel != null)
            uploadingPanel.SetActive(false);

        if (uploadingProgressFill != null)
            uploadingProgressFill.fillAmount = 0f;

        if (emailAlertPanel != null)
            emailAlertPanel.SetActive(false);

        if (emailContentPanel != null)
            emailContentPanel.SetActive(false);

        if (lindaEmailButton != null)
            lindaEmailButton.gameObject.SetActive(false);

        if (submitProjectButton != null)
            submitProjectButton.interactable = true;
    }

    private void ResolveSubmissionReferences()
    {
        if (_submissionReferencesResolved)
            return;

        if (submitProjectButton == null)
            submitProjectButton = FindSceneComponent<Button>("Btn_SubmitProject");

        if (submitProjectButtonText == null && submitProjectButton != null)
            submitProjectButtonText = submitProjectButton.GetComponentInChildren<TextMeshProUGUI>(true);

        if (submissionConfirmationPanel == null)
            submissionConfirmationPanel = FindSceneObject("SubmissionConfirmation");

        if (submissionOkButton == null)
            submissionOkButton = FindChildButton(submissionConfirmationPanel, "Btn_OK", "Btn_Ok", "Button_OK");

        if (uploadingPanel == null)
            uploadingPanel = FindSceneObject("Uploading_Panel");

        if (uploadingProgressFill == null)
            uploadingProgressFill = FindChildImage(uploadingPanel, "Slider", "Fill", "ProgressFill");

        if (emailAlertPanel == null)
            emailAlertPanel = FindSceneObject("Email_alert");

        if (emailAlertRect == null && emailAlertPanel != null)
            emailAlertRect = emailAlertPanel.GetComponent<RectTransform>();

        if (emailAlertOkButton == null)
            emailAlertOkButton = FindChildButton(emailAlertPanel, "Button_OK", "Btn_Ok");

        if (emailWindow == null)
            emailWindow = FindSceneObject("Email");

        if (emailFocusHandler == null && emailWindow != null)
            emailFocusHandler = emailWindow.GetComponent<WindowFocusHandler>();

        if (lindaEmailButton == null)
            lindaEmailButton = FindSceneComponent<Button>("Btn_Linda");

        if (emailContentPanel == null)
            emailContentPanel = FindSceneObject("Email_content");

        if (emailDownloadButton == null)
            emailDownloadButton = FindChildButton(emailContentPanel, "Btn_Virus_Download");

        if (emailDownloadButton == null)
            emailDownloadButton = FindFirstButton(emailContentPanel);

        if (antivirusDownloadController == null)
            antivirusDownloadController = FindSceneComponent<AntivirusDownloadController>();

        if (emailDownloadTarget == null)
            emailDownloadTarget = FindSceneObject("Btn_Virus");

        if (emailAlertRect != null)
        {
            _emailAlertHiddenPosition = emailAlertRect.anchoredPosition;
            _emailAlertShownPosition = _emailAlertHiddenPosition + Vector2.right * emailAlertSlideDistance;
        }

        _submissionReferencesResolved = true;
    }

    private void ResolveEndingReferences()
    {
        if (_endingReferencesResolved && IsButtonInsidePanel(gameSuccessOkButton, gameSuccessPanel))
            return;

        if (gameSuccessPanel == null)
            gameSuccessPanel = FindSceneObject("Game_Success_Panel");

        if (!IsButtonInsidePanel(gameSuccessOkButton, gameSuccessPanel))
            gameSuccessOkButton = null;

        if (gameSuccessOkButton == null)
            gameSuccessOkButton = FindChildButton(gameSuccessPanel, "Btn_OK", "Btn_Ok", "Button_OK");

        if (desktopBackground == null)
        {
            GameObject canvas = FindSceneObject("Canvas");
            Transform background = FindDirectChild(canvas == null ? null : canvas.transform, "Background");
            desktopBackground = background == null ? null : background.GetComponent<Image>();
        }

        if (appIconImages == null || appIconImages.Length == 0)
        {
            appIconImages = new[]
            {
                FindSceneComponent<Image>("Btn_NotePad"),
                FindSceneComponent<Image>("Btn_Notepad"),
                FindSceneComponent<Image>("Btn_Folder"),
                FindSceneComponent<Image>("Btn_searchEngine"),
                FindSceneComponent<Image>("Btn_email")
            };
        }

        if (belugaSprite == null)
            belugaSprite = Resources.Load<Sprite>("Beluga");

        if (toolbar == null)
            toolbar = FindSceneObject("Toolbar");

        if (mainMenu == null)
            mainMenu = FindSceneObject("Main_menu");

        _endingReferencesResolved = true;
    }

    private void CloseOpenDesktopWindows()
    {
        WindowFocusHandler[] windows = Resources.FindObjectsOfTypeAll<WindowFocusHandler>();
        foreach (WindowFocusHandler window in windows)
        {
            if (window != null && window.gameObject.scene.IsValid())
                window.gameObject.SetActive(false);
        }

        if (gameSuccessPanel != null)
            gameSuccessPanel.SetActive(false);

        SetSceneObjectActive("BegulaAntiVIRUS", false);
        SetSceneObjectActive("Platformer-Game", false);
        SetSceneObjectActive("Documents", false);
        SetSceneObjectActive("ImageViewer", false);
        SetSceneObjectActive("Home - Shuffle", false);
        SetSceneObjectActive("Timer", false);
        SetSceneObjectActive("Gameover_panel", false);
    }

    private void ApplyBelugaDesktop()
    {
        if (belugaSprite == null)
            return;

        if (desktopBackground != null)
        {
            desktopBackground.sprite = belugaSprite;
            desktopBackground.preserveAspect = false;
            desktopBackground.color = Color.white;
        }

        if (appIconImages == null)
            return;

        foreach (Image appIconImage in appIconImages)
        {
            if (appIconImage == null)
                continue;

            appIconImage.sprite = belugaSprite;
            appIconImage.preserveAspect = true;
            appIconImage.color = Color.white;
        }
    }

    private void ShowBelugaWarning()
    {
        Transform parent = FindWarningParent();
        if (parent == null)
        {
            CompleteBelugaEnding();
            return;
        }

        GameObject panel = new GameObject("Beluga_Owned_Warning", typeof(RectTransform), typeof(CanvasRenderer));
        panel.transform.SetParent(parent, false);
        panel.transform.SetAsLastSibling();

        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = Vector2.zero;
        panelRect.sizeDelta = new Vector2(560f, 250f);

        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = Color.black;

        CreateWarningText(panelRect);
        CreateWarningOkButton(panelRect, panel);
    }

    private void CompleteBelugaEnding()
    {
        if (mainMenu != null)
        {
            mainMenu.SetActive(true);
            mainMenu.transform.SetAsLastSibling();
        }

        if (toolbar != null)
            toolbar.SetActive(false);

        SetSceneObjectActive("Timer", false);
        SetSceneObjectActive("Gameover_panel", false);
    }

    private Transform FindWarningParent()
    {
        if (gameSuccessPanel != null && gameSuccessPanel.transform.parent != null)
            return gameSuccessPanel.transform.parent;

        GameObject osContainer = FindSceneObject("OS_Container");
        if (osContainer != null)
            return osContainer.transform;

        GameObject canvas = FindSceneObject("Canvas");
        return canvas == null ? null : canvas.transform;
    }

    private void CreateWarningText(RectTransform parent)
    {
        GameObject textObject = new GameObject("Warning_Text", typeof(RectTransform), typeof(CanvasRenderer));
        textObject.transform.SetParent(parent, false);

        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0f, 0.35f);
        textRect.anchorMax = new Vector2(1f, 1f);
        textRect.offsetMin = new Vector2(36f, 0f);
        textRect.offsetMax = new Vector2(-36f, -24f);

        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
        text.text = endingWarningMessage;
        text.fontSize = 34f;
        text.color = Color.white;
        text.alignment = TextAlignmentOptions.Center;
        text.enableWordWrapping = true;
        text.raycastTarget = false;
    }

    private void CreateWarningOkButton(RectTransform parent, GameObject panel)
    {
        GameObject buttonObject = new GameObject("Btn_OK", typeof(RectTransform), typeof(CanvasRenderer));
        buttonObject.transform.SetParent(parent, false);

        RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.5f, 0f);
        buttonRect.anchorMax = new Vector2(0.5f, 0f);
        buttonRect.pivot = new Vector2(0.5f, 0.5f);
        buttonRect.anchoredPosition = new Vector2(0f, 48f);
        buttonRect.sizeDelta = new Vector2(150f, 42f);

        Image buttonImage = buttonObject.AddComponent<Image>();
        buttonImage.color = Color.white;

        Button button = buttonObject.AddComponent<Button>();
        button.targetGraphic = buttonImage;
        button.onClick.AddListener(() =>
        {
            if (panel != null)
                Destroy(panel);

            CompleteBelugaEnding();
        });

        GameObject labelObject = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer));
        labelObject.transform.SetParent(buttonRect, false);

        RectTransform labelRect = labelObject.GetComponent<RectTransform>();
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;

        TextMeshProUGUI label = labelObject.AddComponent<TextMeshProUGUI>();
        label.text = "OK";
        label.fontSize = 26f;
        label.color = Color.black;
        label.alignment = TextAlignmentOptions.Center;
        label.raycastTarget = false;
    }

    private static void OpenWindow(GameObject window, WindowFocusHandler focusHandler)
    {
        if (window == null)
            return;

        if (focusHandler != null)
        {
            focusHandler.OpenWindow(window);
            return;
        }

        window.SetActive(true);
        window.transform.SetAsLastSibling();
    }

    private static void RaiseTransformChain(GameObject gameObject)
    {
        if (gameObject == null)
            return;

        Transform current = gameObject.transform;
        while (current != null)
        {
            current.SetAsLastSibling();
            current = current.parent;
        }
    }

    private static void SetParentsActive(Transform child)
    {
        Transform current = child == null ? null : child.parent;
        while (current != null)
        {
            if (!current.gameObject.activeSelf)
                current.gameObject.SetActive(true);

            current = current.parent;
        }
    }

    private static Button FindChildButton(GameObject parent, params string[] names)
    {
        if (parent == null)
            return null;

        Button[] buttons = parent.GetComponentsInChildren<Button>(true);
        foreach (string name in names)
        {
            foreach (Button button in buttons)
            {
                if (button.name == name)
                    return button;
            }
        }

        return buttons.Length > 0 ? buttons[0] : null;
    }

    private static Button FindFirstButton(GameObject parent)
    {
        if (parent == null)
            return null;

        return parent.GetComponentInChildren<Button>(true);
    }

    private static Image FindChildImage(GameObject parent, params string[] names)
    {
        if (parent == null)
            return null;

        Image[] images = parent.GetComponentsInChildren<Image>(true);
        foreach (string name in names)
        {
            foreach (Image image in images)
            {
                if (image.name == name)
                    return image;
            }
        }

        return images.Length > 0 ? images[0] : null;
    }

    private static T FindSceneComponent<T>(string objectName) where T : Component
    {
        GameObject gameObject = FindSceneObject(objectName);
        return gameObject == null ? null : gameObject.GetComponent<T>();
    }

    private static T FindSceneComponent<T>() where T : Component
    {
        T[] sceneComponents = Resources.FindObjectsOfTypeAll<T>();
        foreach (T sceneComponent in sceneComponents)
        {
            if (sceneComponent.gameObject.scene.IsValid())
                return sceneComponent;
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

    private static Transform FindDirectChild(Transform parent, string childName)
    {
        if (parent == null)
            return null;

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.name == childName)
                return child;
        }

        return null;
    }

    private static bool IsButtonInsidePanel(Button button, GameObject panel)
    {
        if (button == null || panel == null)
            return false;

        Transform current = button.transform;
        while (current != null)
        {
            if (current.gameObject == panel)
                return true;

            current = current.parent;
        }

        return false;
    }

    private static void SetSceneObjectActive(string objectName, bool active)
    {
        GameObject gameObject = FindSceneObject(objectName);
        if (gameObject != null)
            gameObject.SetActive(active);
    }
}
