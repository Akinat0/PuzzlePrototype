using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerViewImageSkin : PlayerViewSkin
{
    [Serializable]
    public struct ImageSkin
    {
        public Sprite Image;
        public PuzzleColorData PuzzleColor;
    }
    
    SpriteRenderer spriteRenderer;

    [SerializeField] ImageSkin[] ImageSkins;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public override void ChangePuzzleSkin(PuzzleColorData puzzleColor)
    {
        spriteRenderer.sprite = ImageSkins.FirstOrDefault(colorSkin => colorSkin.PuzzleColor.ID == puzzleColor.ID).Image;
    }
    
#if UNITY_EDITOR
    
    public ImageSkin[] EditorImageSkins
    {
        get => ImageSkins;
        set => ImageSkins = value;
    }

    [ContextMenu("Setup")]
    void Setup()
    {
        PuzzleColorData[] puzzleColors = GetPuzzleColors();

        ImageSkin[] imageSkins = new ImageSkin[puzzleColors.Length];
        
        for (int i = 0; i < puzzleColors.Length; i++)
            imageSkins[i] = new ImageSkin {Image = null, PuzzleColor = puzzleColors[i]};

        ImageSkins = imageSkins;
    }

#endif
}
