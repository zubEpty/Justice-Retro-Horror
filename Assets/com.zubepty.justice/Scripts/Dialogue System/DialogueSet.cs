using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Dialogue Set")]
public class DialogueSet : ScriptableObject
{
    public List<DialogueLine> lines;
}
