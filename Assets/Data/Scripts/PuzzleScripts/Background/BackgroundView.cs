using System;
using System.Linq;
using Abu.Tools;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundView : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    SpriteMask spriteMask;
    
    SpriteRenderer[] spritesInChildren;
    SpriteRenderer[] SpritesInChildren
    {
        get
        {
            if (spritesInChildren == null)
                spritesInChildren = GetComponentsInChildren<SpriteRenderer>(true);

            return spritesInChildren;
        }
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.localScale = 
            Vector3.one * ScreenScaler.FitHorizontal(spriteRenderer);
        
        CreateClipping();
        
    }

    void CreateClipping()
    {
        spriteMask = gameObject.AddComponent<SpriteMask>();
        spriteMask.sprite = spriteRenderer.sprite;

        foreach (SpriteRenderer sprite in SpritesInChildren)
        {
            if(sprite)
                sprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
    }
}
