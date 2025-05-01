using UnityEngine;

public class PoliceRecordFoundStep : MonoBehaviour
{
    public void PoilceRecordFoundStep(PersonProfile profile)
    {
        if (profile.fullName == "Jaber Molla")
        {
            FindFirstObjectByType<TutorialManager>()?.SetConditionMetExternally();
        }

    }
}
