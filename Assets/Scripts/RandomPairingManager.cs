using UnityEngine;
using UnityEngine.Video;


public class RandomPairingManager : MonoBehaviour
{
    public RoomBundle[] bundles;

    void Start()
    {
        if (bundles == null || bundles.Length == 0)
        {
            Debug.LogError("No bundles assigned in RandomPairingManager!");
            return;
        }

        // Clone array so we can shuffle without losing original mapping
        RoomBundle[] shuffled = (RoomBundle[])bundles.Clone();
        Shuffle(shuffled);

        // Assign shuffled content to fixed triggers
        for (int i = 0; i < bundles.Length; i++)
        {
            var trigger = bundles[i].trigger; // fixed trigger
            var source = shuffled[i];         // randomized content

            if (trigger != null)
            {
                trigger.videoPlayer = source.videoPlayer;
                trigger.manager = source.spawner.conversationManager;
                trigger.conversation = source.conversation;
            }

            Debug.Log($"Room {i} trigger is now paired with {source.bundleName}");
        }
    }

    /// <summary>
    /// Fisher–Yates shuffle
    /// </summary>
    private void Shuffle(RoomBundle[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int j = Random.Range(i, array.Length);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
}