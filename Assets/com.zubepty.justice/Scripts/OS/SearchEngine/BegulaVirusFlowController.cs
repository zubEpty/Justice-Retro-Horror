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
    [SerializeField] private float emailAlertSlideDistance = 520f;
    [SerializeField] private float emailAlertSlideDuration = 0.45f;

    private Vector2 _emailAlertShownPosition;
    private Vector2 _emailAlertHiddenPosition;
    private bool _projectSubmitted;
    private bool _submissionReferencesResolved;
    private bool _submissionListenersHooked;

    private void Awake()
    {
        ResolveSubmissionReferences();

        if (scanButton != null)
            scanButton.onClick.AddListener(ShowLicenseValidator);

        if (helpButton != null)
            helpButton.onClick.AddListener(OpenAntivirusHelpPage);

        if (hackersAddressButton != null)
            hackersAddressButton.onClick.AddListener(OpenPlatformerGame);

        InitializeSubmissionState();
    }

    private void OnEnable()
    {
        HookSubmissionListeners();
    }

    private void OnDestroy()
    {
        if (scanButton != null)
            scanButton.onClick.RemoveListener(ShowLicenseValidator);

        if (helpButton != null)
            helpButton.onClick.RemoveListener(OpenAntivirusHelpPage);

        if (hackersAddressButton != null)
            hackersAddressButton.onClick.RemoveListener(OpenPlatformerGame);

        UnhookSubmissionListeners();
    }

    public void ConfirmProjectSubmission()
    {
        ResolveSubmissionReferences();
        ConfirmSubmission();
    }

    public void OpenSubmittedEmail()
    {
        ResolveSubmissionReferences();
        OpenEmailWindow();
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

        antivirusDownloadController.StartDownload();
    }

    private void InitializeSubmissionState()
    {
        if (submissionConfirmationPanel != null)
            submissionConfirmationPanel.SetActive(false);

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

        if (emailAlertRect != null)
        {
            _emailAlertHiddenPosition = emailAlertRect.anchoredPosition;
            _emailAlertShownPosition = _emailAlertHiddenPosition + Vector2.right * emailAlertSlideDistance;
        }

        _submissionReferencesResolved = true;
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
}
