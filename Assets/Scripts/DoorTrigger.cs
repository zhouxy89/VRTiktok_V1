using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public int roomIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log($"[DoorTrigger] Player entered Room {roomIndex}");
        // Nothing else — RandomizeRooms handles activation
    }
}
