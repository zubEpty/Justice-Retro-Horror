using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FakeProfile", menuName = "FakeSearch/Profile")]
public class FakeProfile : ScriptableObject
{
    public string fullName;
    public string bio;
    public Sprite profileImage;
    public List<string> knownAssociates;
    public List<string> previousPosts; // fake social posts or clues
}
