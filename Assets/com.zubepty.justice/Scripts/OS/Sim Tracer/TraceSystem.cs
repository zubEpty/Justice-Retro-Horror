using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class TraceSystem : MonoBehaviour
{
    public TMP_InputField contactInput;
    public Button submitButton;
    public Transform messageParent;
    public GameObject messagePrefab;
    public ContactProfile[] allProfiles;
    public GameObject MessageWindow;
    public MessageViewer messageViewer; // Add this

    void Start()
    {
        submitButton.onClick.AddListener(CheckContact);
    }

    void CheckContact()
    {
        if (contactInput == null) Debug.Log("rr");
        else
        {
            MessageWindow.SetActive(true);
            string enteredNumber = contactInput.text.Trim();
            ContactProfile profile = allProfiles.FirstOrDefault(p => p.contactNumber == enteredNumber);

            foreach (Transform child in messageParent)
                Destroy(child.gameObject);

            if (profile != null)
            {
                foreach (string msg in profile.messages)
                {
                    GameObject go = Instantiate(messagePrefab, messageParent);
                    go.GetComponent<MessageUI>().Setup(msg, messageViewer);
                }
            }
            else
            {
                Debug.LogWarning("Contact not found!");
            }

        }
          
    }
}
