using UnityEngine;
using UnityEngine.Video;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class AnimationCue
{
    public string stateName = "Shake";
    public int layer = 0;
    public double[] times;
    public string returnIdleOverride = "";
    [HideInInspector] public int nextIndex = 0;
}

public class AvatarVideoAnimatorSync : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Animator[] animators;

    public string idleStateName = "Idle";
    public float crossFade = 0.05f;
    public AnimationCue[] cues;

    private bool prepared = false;
    private bool syncActive = false;

    public void AssignAnimators(List<GameObject> spawnedAvatars)
    {
        animators = new Animator[spawnedAvatars.Count];
        for (int i = 0; i < spawnedAvatars.Count; i++)
        {
            animators[i] = spawnedAvatars[i].GetComponentInChildren<Animator>();
        }
        ResetCues();
    }

    void OnEnable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.prepareCompleted -= OnPrepared;
            videoPlayer.prepareCompleted += OnPrepared;
        }
    }

    void OnDisable()
    {
        if (videoPlayer != null)
            videoPlayer.prepareCompleted -= OnPrepared;
    }

    private void OnPrepared(VideoPlayer vp)
    {
        prepared = true;
        Debug.Log($"[{name}] Video prepared (len≈{vp.length:0.00}s, frameRate={vp.frameRate}), waiting for playback...");
    }


    void Update()
    {
        if (!prepared) return; // ✅ only proceed after prepareCompleted
        if (videoPlayer == null || animators == null || animators.Length == 0) return;

        // ✅ Only activate sync once video is prepared AND playing
        if (!syncActive && prepared && videoPlayer.isPlaying)
        {
            syncActive = true;
            ResetCues();
            Debug.Log($"[{name}] Sync now active, video length={videoPlayer.length:0.00}s, frameRate={videoPlayer.frameRate}");
        }

        if (!syncActive) return;
        if (!videoPlayer.isPlaying) return;

        double currentTime = (videoPlayer.frame >= 0 && videoPlayer.frameRate > 0)
            ? (double)videoPlayer.frame / videoPlayer.frameRate
            : videoPlayer.time;

        foreach (var cue in cues)
        {
            while (cue.nextIndex < cue.times.Length && cue.times[cue.nextIndex] <= currentTime)
            {
                foreach (var anim in animators)
                {
                    if (anim == null || anim.Equals(null)) continue;

                    int stateHash = Animator.StringToHash(cue.stateName);
                    if (!anim.HasState(cue.layer, stateHash))
                    {
                        Debug.LogError($"[{name}] Animator '{anim.name}' has NO state '{cue.stateName}' on layer {cue.layer}");
                        continue;
                    }

                    anim.CrossFade(stateHash, crossFade, cue.layer, 0f);
                    Debug.Log($"[{name}] Played '{cue.stateName}' on {anim.name} at {currentTime:0.00}s");

                    string idle = string.IsNullOrEmpty(cue.returnIdleOverride) ? idleStateName : cue.returnIdleOverride;
                    if (!string.IsNullOrEmpty(idle))
                        StartCoroutine(ReturnToIdleAfter(anim, cue.layer, idle, 1f)); // fallback 1s or use clip length
                }

                cue.nextIndex++;
            }
        }
    }

    private IEnumerator ReturnToIdleAfter(Animator anim, int layer, string idle, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (anim != null && !anim.Equals(null))
        {
            int idleHash = Animator.StringToHash(idle);
            if (anim.HasState(layer, idleHash))
                anim.CrossFade(idleHash, crossFade, layer, 0f);
        }
    }

    public void ResetCues()
    {
        for (int i = 0; i < cues.Length; i++)
            cues[i].nextIndex = 0;
    }

    public void ClearAnimators()
    {
        animators = new Animator[0];
        ResetCues();
        syncActive = false;
        Debug.Log($"[{name}] Cleared animators (avatars destroyed)");
    }
}
