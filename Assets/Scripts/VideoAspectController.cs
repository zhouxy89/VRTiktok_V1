using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoAspectController : MonoBehaviour
{
    [Tooltip("The Quad that displays the video")]
    public Transform videoQuad;

    [Tooltip("How tall should the video screen be in world units")]
    public float targetHeight = 500f;

    private VideoPlayer vp;

    void Start()
    {
        vp = GetComponent<VideoPlayer>();
        vp.prepareCompleted += OnVideoPrepared;
        vp.Prepare(); // triggers the prepareCompleted callback
    }

    private void OnVideoPrepared(VideoPlayer source)
    {
        int width = vp.texture.width;
        int height = vp.texture.height;

        if (height == 0)
        {
            Debug.LogWarning("Video height is 0, cannot calculate aspect ratio.");
            return;
        }

        float aspect = (float)width / height; // width ÷ height
        float newWidth = targetHeight * aspect;
        float newHeight = targetHeight;

        Vector3 newScale = new Vector3(newWidth, newHeight, 1f);

        if (videoQuad != null)
            videoQuad.localScale = newScale;

        Debug.Log($"Video prepared. Size = {width}x{height}, Aspect = {aspect}, New Quad Scale = {newScale}");
    }
}
