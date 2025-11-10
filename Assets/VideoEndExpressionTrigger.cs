using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class VideoEndExpressionTrigger : MonoBehaviour
{
    [Header("References")]
    public VideoPlayer videoPlayer;             // assign in Inspector
    public UMAHeadMotionDriver headDriver;      // assign from avatar prefab in Inspector
    public bool playOnce = true;

    [Header("Motion Mode")]
    public bool triggerShake = true;            // if true → shake
    public bool triggerNod = false;             // if true → nod
    public float duration = 2f;                 // how long to run motion

    private bool fired = false;

    private void OnEnable()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void OnDisable()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        if (playOnce && fired) return;
        fired = true;

        Debug.Log($"[{name}] Video finished — triggering head motion");

        if (headDriver != null)
        {
            if (triggerShake)
                headDriver.StartCoroutine(headDriver.DoShake(duration));

            if (triggerNod)
                headDriver.StartCoroutine(headDriver.DoNod(duration));
        }
    }


    

}
