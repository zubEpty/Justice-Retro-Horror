using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FakeProfile", menuName = "FakeSearch/Profile")]
public class FakeProfile : ScriptableObject
{
    public string fullName;
    public string bio;
    public Sprite profileImage;
    public List<string> knownAssociates;
    public List<ProfilePost> previousPosts; // fake social posts or clues
}


[System.Serializable]
public class ProfilePost
{
    [TextArea]
    public string postText;
    public Sprite postImage; // Optional
}