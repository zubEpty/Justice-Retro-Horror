using UnityEngine;

[CreateAssetMenu(
    fileName = "NewsArticleData",
    menuName = "FakeNews/News Article"
)]
public class NewsArticleData : ScriptableObject
{
    [Header("Article Content")]
    public Sprite newsImage;
    public string title;

    [TextArea(5, 15)]
    public string description;
}