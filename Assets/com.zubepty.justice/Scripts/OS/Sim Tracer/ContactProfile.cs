using UnityEngine;

[CreateAssetMenu(fileName = "New ContactProfile", menuName = "TraceSystem/Contact Profile")]
public class ContactProfile : ScriptableObject
{
    public string contactNumber;
    public string personName;
    public string[] messages;
}
