using UnityEngine;
using UnityEngine.UI;

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

    private void Awake()
    {
        if (scanButton != null)
            scanButton.onClick.AddListener(ShowLicenseValidator);

        if (helpButton != null)
            helpButton.onClick.AddListener(OpenAntivirusHelpPage);

        if (hackersAddressButton != null)
            hackersAddressButton.onClick.AddListener(OpenPlatformerGame);
    }

    private void OnDestroy()
    {
        if (scanButton != null)
            scanButton.onClick.RemoveListener(ShowLicenseValidator);

        if (helpButton != null)
            helpButton.onClick.RemoveListener(OpenAntivirusHelpPage);

        if (hackersAddressButton != null)
            hackersAddressButton.onClick.RemoveListener(OpenPlatformerGame);
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
}
