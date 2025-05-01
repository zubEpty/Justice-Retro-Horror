using UnityEngine;

public class SimTracerFoundStep : MonoBehaviour
{
   public void SetSimTracerStep(ContactProfile profile)
   {
        if (profile.contactNumber == "013435343563")
        {
            FindFirstObjectByType<TutorialManager>()?.SetConditionMetExternally();
        }
   }

    public void SetEvidenceTracedStep(MessageData message)
    {
        if (message.referenceID == "312344")
        {
            FindFirstObjectByType<TutorialManager>()?.SetConditionMetExternally();
        }
    }

}
