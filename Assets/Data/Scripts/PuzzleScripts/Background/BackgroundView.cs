using System;
using System.Linq;
using Abu.Tools;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundView : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] IgnoreList;
    
    SpriteRenderer spriteRenderer;
    SpriteMask spriteMask;
    SortingGroup sortingGroup;
    
    SpriteRenderer[] spritesInChildren;
    SpriteRenderer[] SpritesInChildren => 
        spritesInChildren ?? (spritesInChildren = GetComponentsInChildren<SpriteRenderer>(true));

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.localScale = 
            Vector3.one * ScreenScaler.FitHorizontal(spriteRenderer);
        
        CreateClipping();
    }

    void CreateClipping()
    {
        if (!gameObject.TryGetComponent(out sortingGroup))
            sortingGroup = gameObject.AddComponent<SortingGroup>();
        
        spriteMask = gameObject.AddComponent<SpriteMask>();
        spriteMask.sprite = spriteRenderer.sprite;

        if(IgnoreList == null)
            return;
        
        foreach (SpriteRenderer sprite in SpritesInChildren)
        {
            if(!IgnoreList.Contains(sprite))
                sprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
    }
}