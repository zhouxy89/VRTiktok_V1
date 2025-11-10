using UnityEngine;
using UnityEngine.Video;

public class ConversationTrigger : MonoBehaviour
{
    [Header("Conversation")]
    public ConversationManager manager;       // your existing manager in the scene
    public ConversationData conversation;     // your ConversationData asset

    [Header("Optional Video (play first, then talk)")]
    public VideoPlayer videoPlayer;           // assign if you want video first; leave null to talk immediately
    public bool playOnce = true;

    private bool fired = false;

    private void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false;   // 👈 disable auto-play
            videoPlayer.Stop();               // 👈 ensure it isn’t already running
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("I am conversation Trigger fired by " + other.name + " at time " + Time.time);

        if (!other.CompareTag("Player")) return;
        if (playOnce && fired) return;
        fired = true;

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
            videoPlayer.Play();
        }
        else
        {
            manager.StartConversation(conversation);
        }
    }


    private void OnDestroy()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        vp.loopPointReached -= OnVideoFinished;
        manager.StartConversation(conversation);
    }
}
