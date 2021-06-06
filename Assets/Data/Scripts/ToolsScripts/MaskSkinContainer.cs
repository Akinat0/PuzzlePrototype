using UnityEngine;

[RequireComponent(typeof(SpriteMask))]
public class MaskSkinContainer : SkinContainerBase
{
    SpriteMask spriteMask;
    
    public int Length => _Sprites.Length;

    public int Skin
    {
        get => index;
        set
        {
            if (value < Length && value >= 0)
            {
                index = value;
                UpdateSkin();
            }
            else
                Debug.LogError($"Try to get unavailable skin {value.ToString()}");
            
        }
    }

    void UpdateSkin()
    {
        if (spriteMask == null)
            spriteMask = GetComponent<SpriteMask>();

        spriteMask.sprite = _Sprites[index];
    }
    
#if UNITY_EDITOR
    public static void SetEditorSprites(MaskSkinContainer skinContainer, Sprite[] sprites)
    {
        skinContainer._Sprites = sprites;
    }
#endif
}