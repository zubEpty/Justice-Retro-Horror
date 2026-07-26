using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ProjectSubmissionSubmitButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private BegulaVirusFlowController flowController;

    public void OnPointerClick(PointerEventData eventData)
    {
        Button button = GetComponent<Button>();
        if (button != null && !button.interactable)
            return;

        if (flowController == null)
            flowController = FindFlowController();

        if (flowController != null)
            flowController.StartProjectSubmission();
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
