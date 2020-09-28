using Puzzle;
using PuzzleScripts;
using UnityEngine;

[RequireComponent(typeof(SkinContainer))]
public class SkinPuzzleEnemy : PuzzleEnemy
{

    SkinContainer skinContainer;

    public SkinContainer SkinContainer
    {
        get
        {
            if (skinContainer == null)
                skinContainer = GetComponent<SkinContainer>();

            return skinContainer;
        }
    }
    
    SpriteRenderer spriteRenderer;

    public SpriteRenderer SpriteRenderer
    {
        get
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();
            
            return spriteRenderer;
        }
    }
    
    public override void Instantiate(EnemyParams @params)
    {
        base.Instantiate(@params);
        
        SkinContainer.Skin = @params.stickOut ? 1 : 0;
        
        if(@params.side == Side.Right)
            transform.Rotate(new Vector3(180, 0, 0));
    }
}
