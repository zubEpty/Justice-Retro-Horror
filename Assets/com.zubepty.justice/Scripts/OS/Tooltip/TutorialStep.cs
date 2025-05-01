using UnityEngine;

public enum TutorialConditionType
{
    ButtonClick,
    ConditionMet,
    ManualNext
}

[CreateAssetMenu(fileName = "TutorialStep", menuName = "Tutorial/Step")]
public class TutorialStep : ScriptableObject
{
    [TextArea] public string tutorialText;
    public TutorialConditionType conditionType;
    public string requiredButtonName; // for ButtonClick
}
