using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speaker;
    public string text;
    public string conditionKey; // e.g., "HasKey" or "HelpedNPC"
    public string conditionValue; // e.g., "true"
}
