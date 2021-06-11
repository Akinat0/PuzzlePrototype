using Puzzle;
using PuzzleScripts;
using UnityEngine;
using UnityEngine.UI;

public class NeonPartyShitEnemy : ShitEnemy
{
    
    [SerializeField] private SpriteRenderer ChildSpriteRenderer;

    public override Transform Die()
    {
        Transform effect = base.Die();
        
        effect.localScale = ChildSpriteRenderer.transform.lossyScale;
        effect.localRotation = ChildSpriteRenderer.transform.localRotation;

        ColorProviderComponent colorProvider = effect.GetComponent<ColorProviderComponent>();

        if (colorProvider != null)
            colorProvider.Color = ChildSpriteRenderer.color;
        
        return transform;
    }
}