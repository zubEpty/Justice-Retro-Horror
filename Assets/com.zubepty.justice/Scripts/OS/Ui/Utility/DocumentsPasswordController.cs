using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DocumentsPasswordController : MonoBehaviour
{
    [SerializeField] private Button documentsButton;
    [SerializeField] private GameObject passwordAlertPanel;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button submitButton;
    [SerializeField] private TextMeshProUGUI wrongPasswordText;
    [SerializeField] private GameObject fileDocuments;
    [SerializeField] private WindowFocusHandler fileDocumentsFocusHandler;
    [SerializeField] private BegulaVirusFlowController begulaVirusFlowController;
    [SerializeField] private string password = "password";
    [SerializeField] private bool hideAlertAfterCorrectPassword = true;
    [SerializeField] private bool caseSensitive = true;

    private bool listenersHooked;

    private void Awake()
    {
        ResolveReferences();
        HookListeners();
        HideWrongPassword();
    }

    private void OnEnable()
    {
        ResolveReferences();
        HookListeners();
        ShowPasswordAlert();
    }

    private void OnDestroy()
    {
        UnhookListeners();
    }

    public void ShowPasswordAlert()
    {
        ResolveReferences();

        GameObject alertPanel = passwordAlertPanel != null ? passwordAlertPanel : gameObject;
        alertPanel.SetActive(true);
        BringToFront(alertPanel.transform);
        begulaVirusFlowController?.NotifyPasswordProtectedDocumentsClicked();

        if (passwordInput != null)
        {
            passwordInput.text = string.Empty;
            passwordInput.ActivateInputField();
            EventSystem.current?.SetSelectedGameObject(passwordInput.gameObject);
        }

        HideWrongPassword();
    }

    public void SubmitPassword()
    {
        ResolveReferences();

        string enteredPassword = passwordInput == null ? string.Empty : passwordInput.text;
        string expectedPassword = password ?? string.Empty;
        System.StringComparison comparison = caseSensitive ? System.StringComparison.Ordinal : System.StringComparison.OrdinalIgnoreCase;

        if (!string.Equals(enteredPassword, expectedPassword, comparison))
        {
            ShowWrongPassword();
            BringToFront((passwordAlertPanel != null ? passwordAlertPanel : gameObject).transform);
            return;
        }

        HideWrongPassword();

        GameObject alertPanel = passwordAlertPanel != null ? passwordAlertPanel : gameObject;
        if (hideAlertAfterCorrectPassword)
            alertPanel.SetActive(false);

        OpenFileDocuments();
    }

    private void OpenFileDocuments()
    {
        if (fileDocuments == null)
            return;

        if (fileDocumentsFocusHandler != null)
        {
            fileDocumentsFocusHandler.OpenWindow(fileDocuments);
            return;
        }

        fileDocuments.SetActive(true);
        BringToFront(fileDocuments.transform);
    }

    private void HookListeners()
    {
        if (listenersHooked)
            return;

        if (documentsButton != null)
        {
            documentsButton.onClick.RemoveListener(ShowPasswordAlert);
            documentsButton.onClick.AddListener(ShowPasswordAlert);
        }

        if (submitButton != null)
        {
            submitButton.onClick.RemoveListener(SubmitPassword);
            submitButton.onClick.AddListener(SubmitPassword);
        }

        if (passwordInput != null)
        {
            passwordInput.onSubmit.RemoveListener(SubmitPasswordFromInput);
            passwordInput.onSubmit.AddListener(SubmitPasswordFromInput);
        }

        listenersHooked = true;
    }

    private void UnhookListeners()
    {
        if (documentsButton != null)
            documentsButton.onClick.RemoveListener(ShowPasswordAlert);

        if (submitButton != null)
            submitButton.onClick.RemoveListener(SubmitPassword);

        if (passwordInput != null)
            passwordInput.onSubmit.RemoveListener(SubmitPasswordFromInput);

        listenersHooked = false;
    }

    private void SubmitPasswordFromInput(string _)
    {
        SubmitPassword();
    }

    private void ShowWrongPassword()
    {
        if (wrongPasswordText != null)
            wrongPasswordText.gameObject.SetActive(true);
    }

    private void HideWrongPassword()
    {
        if (wrongPasswordText != null)
            wrongPasswordText.gameObject.SetActive(false);
    }

    private void ResolveReferences()
    {
        if (passwordAlertPanel == null)
            passwordAlertPanel = gameObject.name.Trim() == "Password_Alert_Panel" ? gameObject : FindSceneObject("Password_Alert_Panel");

        GameObject referencePanel = passwordAlertPanel != null ? passwordAlertPanel : gameObject;

        if (documentsButton == null)
            documentsButton = FindSceneComponent<Button>("Btn_Documents");

        if (passwordInput == null)
            passwordInput = referencePanel.GetComponentInChildren<TMP_InputField>(true);

        if (submitButton == null)
            submitButton = FindChildButton(referencePanel, "Btn_Submit", "Button_Submit", "Submit");

        if (wrongPasswordText == null)
            wrongPasswordText = FindChildText(referencePanel, "Wrong Password!!");

        if (fileDocuments == null)
            fileDocuments = FindSceneObject("File_Documents");

        if (fileDocumentsFocusHandler == null && fileDocuments != null)
            fileDocumentsFocusHandler = fileDocuments.GetComponent<WindowFocusHandler>();

        if (begulaVirusFlowController == null)
            begulaVirusFlowController = FindSceneComponent<BegulaVirusFlowController>();
    }

    private static void BringToFront(Transform target)
    {
        Transform current = target;
        while (current != null)
        {
            current.SetAsLastSibling();
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
                if (button.name.Trim() == name)
                    return button;
            }
        }

        return buttons.Length > 0 ? buttons[0] : null;
    }

    private static TextMeshProUGUI FindChildText(GameObject parent, string text)
    {
        if (parent == null)
            return null;

        TextMeshProUGUI[] texts = parent.GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (TextMeshProUGUI textComponent in texts)
        {
            if (textComponent.text.Trim() == text)
                return textComponent;
        }

        return null;
    }

    private static T FindSceneComponent<T>(string objectName) where T : Component
    {
        GameObject gameObject = FindSceneObject(objectName);
        return gameObject == null ? null : gameObject.GetComponent<T>();
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
        GameObject[] gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.name.Trim() == objectName && gameObject.scene.IsValid())
                return gameObject;
        }

        return null;
    }
}
