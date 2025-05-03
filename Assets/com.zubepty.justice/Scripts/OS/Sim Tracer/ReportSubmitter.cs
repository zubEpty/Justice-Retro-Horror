using UnityEngine;
using UnityEngine.Events;

public class ReportSubmitter : MonoBehaviour
{
    public ReportBox reportBox;

    public UnityEvent OnReportSubmitSuccessfully;
    public UnityEvent OnReportSubmitFailed;

    public void SubmitReport()
    {
        if (reportBox.isCorrectEvidence)
        {
            OnReportSubmitSuccessfully.Invoke();

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
