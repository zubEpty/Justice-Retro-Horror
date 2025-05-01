using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class TraceSystem : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField contactInput;
    public Button submitButton;
    public Transform messageParent;
    public GameObject messagePrefab;

    [Header("Data")]
    public ContactProfile[] allProfiles;

    [Header("Functional References")]
    public MessageViewer messageViewer;
    public EvidenceSpawner evidenceSpawner;
    public GameObject MessageWindow;

    [SerializeField] private SimTracerFoundStep _simTracerFoundStep;

    void Start()
    {
        submitButton.onClick.AddListener(CheckContact);
        messageViewer.Initialize(evidenceSpawner);
    }

    void CheckContact()
    {
        if (contactInput == null) Debug.Log("rr");

        else
        {
            MessageWindow.SetActive(true);
          

            string enteredNumber = contactInput.text.Trim();
            ContactProfile profile = allProfiles.FirstOrDefault(p => p.contactNumber == enteredNumber);

            _simTracerFoundStep.SetSimTracerStep(profile);

            // Clear old messages
            foreach (Transform child in messageParent)
                Destroy(child.gameObject);

            if (profile != null)
            {
                foreach (MessageData msgData in profile.messages)
                {
                    GameObject go = Instantiate(messagePrefab, messageParent);
                    MessageUI messageUI = go.GetComponent<MessageUI>();
                    messageUI.Setup(msgData, messageViewer);
                }
            }
            else
            {
                Debug.LogWarning("❌ Contact not found!");
            }
        }
    }
}
