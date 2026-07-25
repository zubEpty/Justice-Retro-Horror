using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SubmittedEmailOpenButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private BegulaVirusFlowController flowController;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!GetComponent<Button>().interactable)
            return;

        if (flowController == null)
            flowController = FindFlowController();

        if (flowController != null)
            flowController.OpenSubmittedEmail();
    }

    private static BegulaVirusFlowController FindFlowController()
    {
        BegulaVirusFlowController[] controllers = Resources.FindObjectsOfTypeAll<BegulaVirusFlowController>();
        foreach (BegulaVirusFlowController controller in controllers)
        {
            if (controller.gameObject.scene.IsValid())
                return controller;
        }

        return null;
    }
}
