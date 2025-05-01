using UnityEngine;

[System.Serializable]
public class MessageData
{
    public string content;
    public string referenceID;
}

[CreateAssetMenu(menuName = "TraceSystem/Contact Profile")]
public class ContactProfile : ScriptableObject
{
    public string contactNumber;
    public string personName;
    public MessageData[] messages;
}
