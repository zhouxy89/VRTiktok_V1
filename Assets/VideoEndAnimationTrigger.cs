using UnityEngine;
using UnityEngine.Video;

public class VideoEndAnimationTrigger : MonoBehaviour
{
    [Header("Animation")]
    public AnimationManager manager;  // reference to AnimationManager
    public string stateName = "Shake";

    [Header("Optional Video (play first, then animate)")]
    public VideoPlayer videoPlayer;
    public bool playOnce = true;

    private bool fired = false;

    private void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false;
            videoPlayer.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("I am animation Trigger fired by " + other.name + " at time " + Time.time);

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
            
            manager.PlayAnimation(stateName);
            
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
        Debug.Log("I am astateName " + stateName);
        manager.PlayAnimation(stateName);
        Debug.Log("I played " + stateName);
    }
}
