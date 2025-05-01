using UnityEngine.EventSystems;
using UnityEngine;

public class ReportBox : MonoBehaviour, IDropHandler
{
    public string requiredReferenceID;
    public bool isCorrectEvidence = false;

    public void OnDrop(PointerEventData eventData)
    {
        EvidenceItem droppedItem = eventData.pointerDrag?.GetComponent<EvidenceItem>();
        if (droppedItem != null)
        {
            isCorrectEvidence = droppedItem.referenceID == requiredReferenceID;
            Debug.Log("Evidence dropped. Match: " + isCorrectEvidence);
        }
    }
}
