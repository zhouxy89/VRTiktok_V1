using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// A bundle that groups together content: Video, Spawner, Conversation, Trigger
/// </summary>
[System.Serializable]
public class RoomBundle
{
    public string bundleName;                 // Label for debugging (e.g. "Yellowstone")
    public VideoPlayer videoPlayer;           // The VideoPlayer for this bundle
    public AvatarSpawner spawner;             // The AvatarSpawner for this bundle
    public ConversationData conversation;     // The ConversationData for this bundle
    public ConversationTrigger trigger;       // The trigger in the scene (fixed location)
}