using UnityEngine;
using UnityEngine.Video;
using System.Collections.Generic;

public class VideoEndAnimation : MonoBehaviour
{
    [Header("References")]
    public VideoPlayer videoPlayer;        // assign in Inspector
    public string stateName = "Shake";     // Animator state to play once video ends
    public int layerIndex = 0;             // Animator layer index (0 = Base Layer)

    private Animator[] animators;          // assigned by spawner

    void OnEnable()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnDisable()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnVideoFinished;
    }

    /// <summary>
    /// Called by AvatarSpawner after avatars are spawned.
    /// Works the same way as ConversationManager.AssignSpeakers().
    /// </summary>
    public void AssignAnimators(List<GameObject> spawnedAvatars)
    {
        animators = new Animator[spawnedAvatars.Count];
        for (int i = 0; i < spawnedAvatars.Count; i++)
        {
            animators[i] = spawnedAvatars[i].GetComponentInChildren<Animator>();
            if (animators[i] == null)
            {
                Debug.LogError($"[{name}] No Animator found on {spawnedAvatars[i].name}");
            }
            else
            {
                Debug.Log($"[{name}] Assigned Animator {animators[i].name}");
            }
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log($"[{name}] Video finished, triggering '{stateName}'");

        if (animators == null || animators.Length == 0)
        {
            Debug.LogWarning($"[{name}] No animators assigned — did AvatarSpawner call AssignAnimators?");
            return;
        }

        foreach (var anim in animators)
        {
            if (anim == null || anim.Equals(null))
            {
                Debug.LogError($"[{name}] Animator reference is NULL in animators array!");
                continue;
            }

            Debug.Log($"[{name}] Animator found: {anim.name}");
            DebugListStates(anim);

            int stateHash = Animator.StringToHash(stateName);
            if (anim.HasState(layerIndex, stateHash))
            {
                anim.Play(stateHash, layerIndex, 0f);
                Debug.Log($"[{name}] Played state '{stateName}' on {anim.name}");
            }
            else
            {
                Debug.LogError($"[{name}] Animator '{anim.name}' has NO state '{stateName}' in layer {layerIndex}");
            }
        }


    }

    private void DebugListStates(Animator anim)
    {
        var controller = anim.runtimeAnimatorController;
        if (controller == null)
        {
            Debug.LogError($"Animator {anim.name} has no controller assigned!");
            return;
        }

        foreach (var clip in controller.animationClips)
        {
            Debug.Log($"Animator {anim.name} has clip: {clip.name}");
        }
    }

}
