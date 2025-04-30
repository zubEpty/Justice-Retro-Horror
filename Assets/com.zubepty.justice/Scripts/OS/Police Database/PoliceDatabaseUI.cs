using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PoliceDatabaseUI : MonoBehaviour
{
    [Header("Database")]
    [SerializeField] private PersonDatabase _personDatabase;

    [Header("Input Fields")]
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private TMP_InputField _identifierInput; // NID / DOB / License No

    [Header("Output Fields")]
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _dobText;
    [SerializeField] private TextMeshProUGUI _nidText;
    [SerializeField] private TextMeshProUGUI _licenseText;
    [SerializeField] private Image _photo;
    [SerializeField] private TextMeshProUGUI _criminalStatusText;   
    [SerializeField] private TextMeshProUGUI _criminalRecordsText;

    [SerializeField] private GameObject _alertPanel;
    [SerializeField] private GameObject _profilePanel;
    [SerializeField] private TextMeshProUGUI _alertText;

    /// <summary>
    /// Called when search button is pressed. Requires name and one identifier to find profile.
    /// </summary>
    public void SearchProfile()
    {
        string name = _nameInput.text.Trim();
        string identifier = _identifierInput.text.Trim();

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(identifier))
        {
            ClearDisplay("Please enter valid data.");
            return;
        }

        PersonProfile result = _personDatabase.FindProfileByNameAndIdentifier(name, identifier);

        if (result != null)
        {
            DisplayPersonInfo(result);
        }
        else
        {
            ClearDisplay("No matching person found.");
        }
    }

 


    private void DisplayPersonInfo(PersonProfile person)
    {
        _profilePanel.SetActive(true);
        _nameText.text = $"{person.fullName}";
        _dobText.text = $"{person.dateOfBirth}";
        _nidText.text = $"{person.nationalID}";
        _licenseText.text = $"{person.licenseNumber}";
        _photo.sprite = person.photo;

        bool hasCriminals = person.HasCriminalRecord;
        _criminalStatusText.text = $"{(hasCriminals ? "Yes" : "No")}";

        _criminalRecordsText.text = hasCriminals
            ? "Details:\n- " + string.Join("\n- ", person.criminalRecords)
            : "";
    }



    private void ClearDisplay(string message)
    {
        _alertPanel.SetActive(true);
        _alertText.text = message;
        _profilePanel.SetActive(false);
    }
}
