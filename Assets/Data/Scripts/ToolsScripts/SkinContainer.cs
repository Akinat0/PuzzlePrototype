using UnityEngine;

public class SkinContainer : SkinContainerBase
{
    private int index = 0;
    private SpriteRenderer _spriteRenderer;
    
    public int Length => _Sprites.Length;

    public int Skin
    {
        get => index;
        set
        {
            if (value < Length)
                index = value;
            else
                Debug.LogError("Try to get unavailable skin");
            UpdateSkin();
        }
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void UpdateSkin()
    {
        _spriteRenderer.sprite = _Sprites[index];
    }
}
