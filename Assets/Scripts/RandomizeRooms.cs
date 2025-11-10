using UnityEngine;
using UnityEngine.Video;

public class RandomizeRooms : MonoBehaviour
{
    public GameObject[] rooms;
    public float roomSpacing = 6.23f;

    private int currentRoomIndex = 0;

    void Start()
    {
        if (rooms == null || rooms.Length == 0)
        {
            Debug.LogError("No rooms assigned!");
            return;
        }

        Shuffle(rooms);

        // deactivate all first
        foreach (var r in rooms)
            r.SetActive(false);

        // show first 2
        ActivateRoom(0, Vector3.zero);
        if (rooms.Length > 1)
            ActivateRoom(1, new Vector3(roomSpacing, 0, 0));
    }

    private void Shuffle(GameObject[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int j = Random.Range(i, array.Length);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }

    private void ActivateRoom(int index, Vector3 position)
    {
        if (index < 0 || index >= rooms.Length) return;

        // enable the room itself
        rooms[index].SetActive(true);
        rooms[index].transform.position = position;

        // 🔹 spawn avatars
        var spawner = rooms[index].GetComponentInChildren<AvatarSpawner>();
        if (spawner != null)
            spawner.SpawnAvatars();

        // 🔹 enable video planes + triggers (don’t auto-play yet)
        var videoTriggers = rooms[index].GetComponentsInChildren<VideoTrigger>(true);
        foreach (var trigger in videoTriggers)
        {
            trigger.gameObject.SetActive(true);
            Debug.Log($"Enabled VideoTrigger {trigger.name} in {rooms[index].name}");
        }

        var videos = rooms[index].GetComponentsInChildren<UnityEngine.Video.VideoPlayer>(true);
        foreach (var v in videos)
        {
            v.gameObject.SetActive(true); // so trigger can control it
            Debug.Log($"Activated VideoPlayer {v.name} in {rooms[index].name}");
        }
    }


    public void AdvanceToNextRoom()
    {
        int nextIndex = currentRoomIndex + 2;
        int deactivateIndex = currentRoomIndex;

        // 🔹 deactivate and clean up oldest room
        if (deactivateIndex < rooms.Length)
        {
            var oldRoom = rooms[deactivateIndex];

            // call spawner.DestroyAvatars() if it exists
            var oldSpawner = oldRoom.GetComponentInChildren<AvatarSpawner>();
            if (oldSpawner != null)
                oldSpawner.DestroyAvatars();

            oldRoom.SetActive(false);
            Debug.Log($"Deactivated and cleaned up {oldRoom.name}");
        }

        // 🔹 activate next room
        if (nextIndex < rooms.Length)
        {
            Vector3 pos = new Vector3(nextIndex * roomSpacing, 0, 0);
            ActivateRoom(nextIndex, pos);
        }

        currentRoomIndex++;
    }

}
