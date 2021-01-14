using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerViewColorSkin : MonoBehaviour
{
    [Serializable]
    public struct ColorSkin
    {
        public Color Color;
        public PuzzleColorData PuzzleColor;
    }
    
    SpriteRenderer spriteRenderer;

    [SerializeField] ColorSkin[] ColorSkins;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangePuzzleSkin(PuzzleColorData puzzleColor)
    {
        spriteRenderer.color = ColorSkins.FirstOrDefault(colorSkin => colorSkin.PuzzleColor.ID == puzzleColor.ID).Color;
    }
    
    
#if UNITY_EDITOR
    
    public ColorSkin[] EditorColorSkins
    {
        get => ColorSkins;
        set => ColorSkins = value;
    }

#endif
}
