using UnityEngine;

public class HideTrigger : MonoBehaviour
{
    private RandomizeRooms manager;

    void Start()
    {
        manager = FindObjectOfType<RandomizeRooms>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (manager != null)
        {
            manager.AdvanceToNextRoom();
            Debug.Log("[HideTrigger] Player left room, advancing to next room");
        }
    }
}
