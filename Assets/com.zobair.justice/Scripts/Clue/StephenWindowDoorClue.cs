using UnityEngine;

public class StephenWindowDoorClue : ClueBase
{
    public override void LeaveClue()
    {
        Debug.LogError("Opening Door or Window");
    }

    public override void SetEnvironment()
    {
        Debug.LogError("Starting to Open Door or Window");
    }
    
}
