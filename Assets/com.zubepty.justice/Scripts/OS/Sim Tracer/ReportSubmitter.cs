using UnityEngine;

public class ReportSubmitter : MonoBehaviour
{
    public ReportBox reportBox;

    public void SubmitReport()
    {
        if (reportBox.isCorrectEvidence)
        {
            Debug.Log("✅ Correct Evidence Submitted!");
            CompleteTutorial();
        }
        else
        {
            Debug.Log("❌ Incorrect Evidence!");
        }
    }

    public void CompleteTutorial()
    {
        FindFirstObjectByType<TutorialManager>()?.SetConditionMetExternally();
    }

}
