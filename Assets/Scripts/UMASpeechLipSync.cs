using UnityEngine;
using UMA.PoseTools;
using UMA.CharacterSystem;

[RequireComponent(typeof(AudioSource))]
public class UMASpeechLipSync_OnAudioFilter : MonoBehaviour
{
    [Header("Lip Sync")]
    [Tooltip("Multiplier from audio RMS to jaw open amount.")]
    public float sensitivity = 30f;
    [Tooltip("Higher = snappier jaw; lower = smoother.")]
    public float smoothSpeed = 12f;

    private AudioSource src;                  // your existing (3D) AudioSource
    private UMAExpressionPlayer expr;
    private DynamicCharacterAvatar avatar;

    // audio thread -> main thread handoff
    private volatile float latestRms = 0f;    // updated in OnAudioFilterRead
    private float jawTarget = 0f;

    void Awake()
    {
        src = GetComponent<AudioSource>();
        expr = GetComponent<UMAExpressionPlayer>();
        avatar = GetComponent<DynamicCharacterAvatar>();

        if (!expr) Debug.LogWarning($"{name}: UMAExpressionPlayer missing.");
        if (!src)  Debug.LogWarning($"{name}: AudioSource missing.");

        // If UMA rebuilds the character at runtime, re-enable jaw override afterward.
        if (avatar != null)
        {
            avatar.CharacterUpdated.AddListener(_ =>
            {
                if (expr != null) expr.overrideMecanimJaw = true;
            });
        }
    }

    // Called on the audio thread for THIS AudioSource before mixing/spatialization.
    void OnAudioFilterRead(float[] data, int channels)
    {
        if (data == null || data.Length == 0) { latestRms = 0f; return; }

        // Compute RMS across frames (combine channels to mono)
        double sumSq = 0.0;
        int frames = data.Length / Mathf.Max(1, channels);

        for (int f = 0; f < frames; f++)
        {
            // average all channels for this frame
            float s = 0f;
            int baseIdx = f * channels;
            for (int c = 0; c < channels; c++) s += data[baseIdx + c];
            s /= channels;
            sumSq += (double)s * (double)s;
        }

        float rms = Mathf.Sqrt((float)(sumSq / frames));
        latestRms = rms; // hand off to Update()
    }

    void Update()
    {
        if (expr == null) return;

        // Force UMA to allow jaw channel even if no Humanoid Jaw bone is mapped
        expr.overrideMecanimJaw = true;

        // If the clip isn’t playing, close the mouth
        if (src == null || !src.isPlaying)
        {
            jawTarget = 0f;
        }
        else
        {
            // Map raw RMS -> [0..1]
            jawTarget = Mathf.Clamp01(latestRms * sensitivity);
        }

        // Smooth the jaw motion
        expr.jawOpen_Close = Mathf.Lerp(expr.jawOpen_Close, jawTarget, Time.deltaTime * smoothSpeed);
    }
}
