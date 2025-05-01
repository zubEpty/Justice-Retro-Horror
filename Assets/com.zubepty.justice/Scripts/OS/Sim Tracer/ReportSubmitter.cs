using UnityEngine;

public class ReportSubmitter : MonoBehaviour
{
    public ReportBox reportBox;

    public void SubmitReport()
    {
        if (reportBox.isCorrectEvidence)
        {
            Debug.Log("✅ Correct Evidence Submitted!");
        }
        else
        {
            Debug.Log("❌ Incorrect Evidence!");
        }
    }
}
