using UnityEngine;
using UnityEngine.Video;

public class VideoCueHeadMotion : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public UMAHeadMotionDriver headDriver;

    [System.Serializable]
    public class Cue
    {
        public double time;          // when to trigger (in seconds)
        public bool shake;           // shake or nod
        public float duration = 2f;  // how long
    }

    public Cue[] cues;

    private int nextCue = 0;

    void Update()
    {
        if (videoPlayer == null || headDriver == null || cues == null || cues.Length == 0) return;
        if (!videoPlayer.isPlaying) return;

        double currentTime = videoPlayer.time;

        if (nextCue < cues.Length && currentTime >= cues[nextCue].time)
        {
            Cue cue = cues[nextCue];

            if (cue.shake)
                headDriver.StartCoroutine(headDriver.DoShake(cue.duration));
            else
                headDriver.StartCoroutine(headDriver.DoNod(cue.duration));

            Debug.Log($"[{name}] Triggered head motion at {cue.time}s for {cue.duration}s");

            nextCue++;
        }
    }
}
