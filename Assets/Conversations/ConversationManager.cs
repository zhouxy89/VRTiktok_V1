using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ConversationManager : MonoBehaviour
{
    [Tooltip("AudioSources of the spawned avatars (0 = Avatar1, 1 = Avatar2, etc.)")]
    public AudioSource[] speakers;

    private Coroutine currentConversation;

    /// <summary>
    /// Called by AvatarSpawner after avatars are spawned.
    /// This sets the speakers array to the avatars' AudioSources.
    /// </summary>
    public void AssignSpeakers(List<GameObject> spawnedAvatars)
    {
        speakers = new AudioSource[spawnedAvatars.Count];
        for (int i = 0; i < spawnedAvatars.Count; i++)
        {
            speakers[i] = spawnedAvatars[i].GetComponentInChildren<AudioSource>();
            if (speakers[i] == null)
            {
                Debug.LogError($"No AudioSource found on {spawnedAvatars[i].name}");
            }
        }
    }

    public void StartConversation(ConversationData convo)
    {
        if (currentConversation != null)
            StopCoroutine(currentConversation);

        currentConversation = StartCoroutine(PlayConversation(convo));
    }

    public void StopConversation()
    {
        if (currentConversation != null)
        {
            StopCoroutine(currentConversation);
            currentConversation = null;
        }

        if (speakers != null)
        {
            foreach (var speaker in speakers)
            {
                if (speaker != null)
                    speaker.Stop();
            }
        }
    }

    private IEnumerator PlayConversation(ConversationData convo)
    {
        if (speakers == null || speakers.Length == 0)
        {
            Debug.LogError("Speakers not assigned! Make sure AvatarSpawner calls AssignSpeakers.");
            yield break;
        }

        foreach (var line in convo.lines)
        {
            AudioSource currentSpeaker = speakers[line.speakerIndex];

            string path = GetStreamingAssetsPath("Conversations/" + line.fileName);
            Debug.Log("Loading audio from: " + path);

            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Failed to load audio: " + path + " Error: " + www.error);
                }
                else
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(www);

                    // Ensure AudioSource is enabled before playing
                    if (!currentSpeaker.gameObject.activeInHierarchy)
                        currentSpeaker.gameObject.SetActive(true);
                    if (!currentSpeaker.enabled)
                        currentSpeaker.enabled = true;

                    currentSpeaker.clip = clip;
                    currentSpeaker.Play();

                    yield return new WaitForSeconds(clip.length + convo.pauseBetweenLines);

                    Destroy(clip); // free memory
                }
            }
        }

        currentConversation = null;
    }

    private string GetStreamingAssetsPath(string relativePath)
    {
        string fullPath = System.IO.Path.Combine(Application.streamingAssetsPath, relativePath);

#if UNITY_ANDROID && !UNITY_EDITOR
    // On Android (Quest), do not prepend file://
    return fullPath;
#else
        // On desktop platforms, UnityWebRequest needs file:// prefix
        if (!fullPath.StartsWith("file://"))
            fullPath = "file://" + fullPath;
        return fullPath;
#endif
    }

}
