using UnityEngine;
using UMA.PoseTools;
using UMA.CharacterSystem;
using System.Collections;

[RequireComponent(typeof(UMAExpressionPlayer))]
public class UMAHeadMotionDriver : MonoBehaviour
{
    [Header("Motion Settings")]
    [Tooltip("How fast the head oscillates for shake/nod.")]
    public float frequency = 2f; // cycles per second

    [Tooltip("How far to rotate head (0.0 = still, 1.0 = max UMA pose).")]
    [Range(0f, 1f)]
    public float amplitude = 0.3f;

    [Tooltip("How quickly to smooth transitions.")]
    public float smoothSpeed = 5f;

    private UMAExpressionPlayer expr;
    private DynamicCharacterAvatar avatar;

    // mode flags
    private bool shake = false;
    private bool nod = false;

    void Awake()
    {
        expr = GetComponent<UMAExpressionPlayer>();
        avatar = GetComponent<DynamicCharacterAvatar>();

        if (!expr) Debug.LogWarning($"{name}: UMAExpressionPlayer missing.");

        // If UMA rebuilds the character at runtime, re-enable overrides
        if (avatar != null)
        {
            avatar.CharacterUpdated.AddListener(_ =>
            {
                if (expr != null)
                {
                    expr.overrideMecanimHead = true;
                    expr.overrideMecanimNeck = true;
                }
            });
        }
    }

    void Update()
    {
        if (expr == null) return;

        expr.overrideMecanimHead = true;
        expr.overrideMecanimNeck = true;

        float targetValue = 0f;

        if (shake)
        {
            targetValue = Mathf.Sin(Time.time * frequency * Mathf.PI * 2f) * amplitude;
            expr.headLeft_Right = Mathf.Lerp(expr.headLeft_Right, targetValue, Time.deltaTime * smoothSpeed);
        }
        else if (nod)
        {
            targetValue = Mathf.Sin(Time.time * frequency * Mathf.PI * 2f) * amplitude;
            expr.headUp_Down = Mathf.Lerp(expr.headUp_Down, targetValue, Time.deltaTime * smoothSpeed);
        }
        else
        {
            // reset if no motion
            expr.headLeft_Right = Mathf.Lerp(expr.headLeft_Right, 0f, Time.deltaTime * smoothSpeed);
            expr.headUp_Down = Mathf.Lerp(expr.headUp_Down, 0f, Time.deltaTime * smoothSpeed);
        }
    }

    // 👉 Public coroutines to start timed shake/nod
    public IEnumerator DoShake(float duration)
    {
        shake = true;
        yield return new WaitForSeconds(duration);
        shake = false;
    }

    public IEnumerator DoNod(float duration)
    {
        nod = true;
        yield return new WaitForSeconds(duration);
        nod = false;
    }
}
