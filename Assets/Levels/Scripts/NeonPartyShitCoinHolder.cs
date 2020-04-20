using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonPartyShitCoinHolder : CoinHolder
{
    public SpriteRenderer spriteRenderer;
    protected override void CreateEffects()
    {
        base.CreateEffects();
        
        DestroyImmediate(mask);
        mask = spriteRenderer.gameObject.AddComponent<SpriteMask>();
        mask.sprite = spriteRenderer.sprite;
    }
}
