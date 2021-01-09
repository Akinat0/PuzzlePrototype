using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerViewColorSkin : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    [SerializeField] List<Color> colors;
    [SerializeField] List<string> colorIDs;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangePuzzleSkin(PuzzleColorData puzzleColor)
    {
        int colorIndex = colorIDs.IndexOf(puzzleColor.ID);

        if (colorIndex < 0)
        {
            Debug.LogError($"[PlayerView] Color {puzzleColor.ID} doesn't exist. It won't be updated.");
            return;
        }

        spriteRenderer.color = colors[colorIndex];
    }
    
    
#if UNITY_EDITOR

    [Obsolete("For editor only", false)]
    public List<Color> Colors
    {
        get => colors;
        set => colors = value;
    }

    [Obsolete("For editor only", false)]
    public List<string> ColorIDs
    {
        get => colorIDs;
        set => colorIDs = value;
    }

#endif
}
