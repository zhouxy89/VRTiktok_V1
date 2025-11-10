using UnityEngine;
using UMA.PoseTools;
using System.Collections.Generic;

public class ExpressionManager : MonoBehaviour
{
    private UMAExpressionPlayer[] expressionPlayers;

    /// <summary>
    /// Called by AvatarSpawner after avatars are spawned.
    /// Mirrors ConversationManager.AssignSpeakers().
    /// </summary>
    public void AssignExpressions(List<GameObject> spawnedAvatars)
    {
        expressionPlayers = new UMAExpressionPlayer[spawnedAvatars.Count];
        for (int i = 0; i < spawnedAvatars.Count; i++)
        {
            expressionPlayers[i] = spawnedAvatars[i].GetComponentInChildren<UMAExpressionPlayer>();
            if (expressionPlayers[i] == null)
                Debug.LogError($"No UMAExpressionPlayer found on {spawnedAvatars[i].name}");
            else
                Debug.Log($"Assigned UMAExpressionPlayer on {spawnedAvatars[i].name}");
        }
    }

    /// <summary>
    /// Shake heads for all assigned avatars.
    /// </summary>
    public void PlayShake(float duration = 1f)
    {
        if (expressionPlayers == null) return;
        foreach (var expr in expressionPlayers)
        {
            if (expr != null) expr.StartCoroutine(DoShake(expr, duration));
        }
    }

    /// <summary>
    /// Nod heads for all assigned avatars.
    /// </summary>
    public void PlayNod(float duration = 1f)
    {
        if (expressionPlayers == null) return;
        foreach (var expr in expressionPlayers)
        {
            if (expr != null) expr.StartCoroutine(DoNod(expr, duration));
        }
    }

    private System.Collections.IEnumerator DoShake(UMAExpressionPlayer expr, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            expr.headLeft_Right = Mathf.Sin(Time.time * 15f) * 0.3f;
            yield return null;
        }
        expr.headLeft_Right = 0f;
    }

    private System.Collections.IEnumerator DoNod(UMAExpressionPlayer expr, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            expr.headUp_Down = Mathf.Sin(Time.time * 10f) * 0.25f;
            yield return null;
        }
        expr.headUp_Down = 0f;
    }
}
