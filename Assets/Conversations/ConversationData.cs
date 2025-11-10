using UnityEngine;

[System.Serializable]
public class ConversationLine
{
    [Tooltip("0 = first avatar, 1 = second avatar")]
    public int speakerIndex;

    [Tooltip("Relative filename in StreamingAssets/Audio, e.g. 'Room1_Line1.mp3'")]
    public string fileName;
}


[CreateAssetMenu(fileName = "NewConversation", menuName = "Dialogue/Conversation")]
public class ConversationData : ScriptableObject
{
    public ConversationLine[] lines;
    public float pauseBetweenLines = 0.3f;
}
