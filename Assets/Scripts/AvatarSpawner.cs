using UnityEngine;
using System.Collections.Generic;

public class AvatarSpawner : MonoBehaviour
{
    [Header("Assign avatar prefabs here")]
    public GameObject[] avatarPrefabs;

    [Header("Spawn points for avatars (match index to prefab)")]
    public Transform[] spawnPoints;

    [Header("Conversation Manager for this room")]
    public ConversationManager conversationManager; // assign in Inspector

    [Header("Video/Animation Sync for this room")]
    public AvatarVideoAnimatorSync syncScript; // assign in Inspector

    [Header("Animation after video ends")]
    public VideoEndAnimation videoEndAnimation;   // assign in Inspector

    public AnimationManager animationManager;

    public ExpressionManager expressionManager;
    public VideoEndExpressionTrigger videoEndExpressionTrigger;

    [Header("Video Cues for Head Motion")]
    public VideoCueHeadMotion videoCueHeadMotion;




    private List<GameObject> spawnedAvatars = new List<GameObject>();

    public void SpawnAvatars()
    {
        if (spawnedAvatars.Count > 0)
        {
            Debug.LogWarning("Avatars already spawned, skipping duplicate spawn.");
            return;
        }

        for (int i = 0; i < avatarPrefabs.Length; i++)
        {
            if (avatarPrefabs[i] != null && spawnPoints[i] != null)
            {
                GameObject avatar = Instantiate(
                    avatarPrefabs[i],
                    spawnPoints[i].position,
                    spawnPoints[i].rotation
                );
                spawnedAvatars.Add(avatar);

                if (syncScript != null)
                {
                    syncScript.ClearAnimators();                 // drop any previous-room refs
                    syncScript.AssignAnimators(spawnedAvatars);  // hand in only avatars of THIS spawn
                    Debug.Log($"[{name}] Assigned {spawnedAvatars.Count} animators to {syncScript.name}");
                }
                else
                {
                    Debug.LogWarning($"[{name}] No AvatarVideoAnimatorSync linked on this room.");
                }
            }
        }

        if (conversationManager != null)
        {
            conversationManager.AssignSpeakers(spawnedAvatars);
            Debug.Log($"Assigned {spawnedAvatars.Count} speakers to ConversationManager.");
        }

        
        if (videoEndExpressionTrigger != null)
        {
            var exprDriver = spawnedAvatars[0].GetComponentInChildren<UMAHeadMotionDriver>();
            videoEndExpressionTrigger.headDriver = exprDriver;
        }

        if (videoCueHeadMotion != null)
        {
            var driver = spawnedAvatars[0].GetComponentInChildren<UMAHeadMotionDriver>();
            if (driver != null)
            {
                videoCueHeadMotion.headDriver = driver;
                Debug.Log($"[{name}] Assigned head driver {driver.name} to VideoCueHeadMotion");
            }
        }


    }

    public void DestroyAvatars()
    {
        foreach (var avatar in spawnedAvatars)
        {
            if (avatar != null)
                Destroy(avatar);
        }
        spawnedAvatars.Clear();

        if (conversationManager != null)
            conversationManager.speakers = null;

        if (syncScript != null)
            syncScript.ClearAnimators();
        
    }
}
