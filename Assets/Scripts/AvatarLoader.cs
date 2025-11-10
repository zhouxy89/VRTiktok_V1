using UnityEngine;
using UMA;
using UMA.CharacterSystem;

public class AvatarLoader : MonoBehaviour
{
    [Header("References")]
    public DynamicCharacterAvatar avatar;   // drag the DynamicCharacterAvatar from your prefab
    public UMATextRecipe startRecipe;       // drag the .asset you saved with "Save as Asset (runtime only)"

    void Start()
    {
        if (avatar != null && startRecipe != null)
        {
            // Load the saved recipe directly into the avatar
            avatar.LoadFromRecipe(startRecipe);
        }
        else
        {
            Debug.LogWarning("AvatarLoader: Missing avatar or recipe asset!");
        }
    }
}
