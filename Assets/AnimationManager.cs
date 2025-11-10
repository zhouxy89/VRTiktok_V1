using UnityEngine;
using System.Collections.Generic;

public class AnimationManager : MonoBehaviour
{
    private Animator[] animators;
    [Header("Dedicated Controllers")]
    public RuntimeAnimatorController shakeController; // assign ShakeController in Inspector


    /// <summary>
    /// Called by AvatarSpawner after avatars are spawned.
    /// Mirrors ConversationManager.AssignSpeakers().
    /// </summary>
    public void AssignAnimators(List<GameObject> spawnedAvatars)
    {
        animators = new Animator[spawnedAvatars.Count];
        for (int i = 0; i < spawnedAvatars.Count; i++)
        {
            animators[i] = spawnedAvatars[i].GetComponentInChildren<Animator>();
            if (animators[i] == null)
                Debug.LogError($"No Animator found on {spawnedAvatars[i].name}");
            else
                Debug.Log($"Assigned Animator {animators[i].name}");
        }
    }

    public void SwapToController(RuntimeAnimatorController controller)
    {
        if (animators == null || animators.Length == 0)
        {
            Debug.LogWarning($"[{name}] No animators assigned!");
            return;
        }

        foreach (var anim in animators)
        {
            if (anim != null && !anim.Equals(null))
            {
                anim.runtimeAnimatorController = controller;
                Debug.Log($"[{name}] Swapped {anim.name} to {controller.name}");
            }
        }
    }


    /// <summary>
    /// Play a given state on all assigned animators.
    /// </summary>
    public void PlayAnimation(string stateName, int layer = 0)
    {
        if (animators == null || animators.Length == 0)
        {
            Debug.LogWarning("No animators assigned!");
            return;
        }

        foreach (var anim in animators)
        {
            if (anim != null && !anim.Equals(null))
            {
                int hash = Animator.StringToHash(stateName);
                if (anim.HasState(layer, hash))
                {
                    anim.Play(hash, layer, 0f);
                    Debug.Log($"Played state '{stateName}' on {anim.name}");
                }
                else
                {
                    Debug.LogError($"Animator '{anim.name}' has NO state '{stateName}' in layer {layer}");
                }
            }
        }
    }
}
