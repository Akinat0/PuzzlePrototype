﻿using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerViewColorSkin : PlayerViewSkin
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

    public override void ChangePuzzleSkin(PuzzleColorData puzzleColor)
    {
        spriteRenderer.color = ColorSkins.FirstOrDefault(colorSkin => colorSkin.PuzzleColor.ID == puzzleColor.ID).Color;
    }
    
    
#if UNITY_EDITOR
    
    public ColorSkin[] EditorColorSkins
    {
        get => ColorSkins;
        set => ColorSkins = value;
    }

    [ContextMenu("Setup")]
    void Setup()
    {
        PuzzleColorData[] puzzleColors = GetPuzzleColors();

        ColorSkin[] colorSkins = new ColorSkin[puzzleColors.Length];
        
        for (int i = 0; i < puzzleColors.Length; i++)
            colorSkins[i] = new ColorSkin {Color = Color.white, PuzzleColor = puzzleColors[i]};

        ColorSkins = colorSkins;
    }
#endif
}
