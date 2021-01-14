using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SkinContainer : SkinContainerBase
{
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
                Debug.LogError("Try to get unavailable skin " + value);
            
        }
    }

    private void UpdateSkin()
    {
        if (_SpriteRenderer == null)
            _SpriteRenderer = GetComponent<SpriteRenderer>();

        try
        {
            _SpriteRenderer.sprite = _Sprites[index];
        }
        catch
        {
            Debug.LogError("Index " + index);
        }
    }
    
    #if UNITY_EDITOR
    public static void SetEditorSprites(SkinContainer skinContainer, Sprite[] sprites)
    {
        skinContainer._Sprites = sprites;
    }
    #endif
}
