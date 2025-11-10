using UnityEngine;
using UnityEngine.Video;

public class VideoTrigger : MonoBehaviour
{
    [Header("Video Player to control")]
    public VideoPlayer videoPlayer;

    [Header("Video file name inside StreamingAssets folder")]
    public string videoFileName; // e.g. "01_Misinfo_YellowStoneAnimals_Video.mp4"

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && videoPlayer != null)
        {
            // Build the correct path for Quest (Android) and Editor
            string path = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);

            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = path;

            Debug.Log("Playing video from: " + path);
            videoPlayer.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && videoPlayer != null)
        {
            videoPlayer.Stop();
            Debug.Log("Video stopped");
        }
    }
}
