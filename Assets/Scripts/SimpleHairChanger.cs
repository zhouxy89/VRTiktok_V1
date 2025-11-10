using UnityEngine;
using UMA.CharacterSystem;

public class SimpleHairChanger : MonoBehaviour
{
    private DynamicCharacterAvatar avatar;

    void Start()
    {
        avatar = GetComponent<DynamicCharacterAvatar>();
    }

    // Change hairstyle (Wardrobe Recipe name must exist in UMA library)
    public void ChangeHairStyle(string styleName)
    {
        avatar.SetSlot("Hair", styleName);
        avatar.BuildCharacter();
    }

    // Change hair color
    public void ChangeHairColor(Color newColor)
    {
        avatar.SetColor("Hair", newColor);
        avatar.BuildCharacter();
    }
}
