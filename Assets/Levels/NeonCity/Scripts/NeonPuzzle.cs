using Puzzle;
using PuzzleScripts;
using UnityEngine;

public class NeonPuzzle : PuzzleEnemy
{
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
        SpriteRenderer.flipX = !@params.stickOut;
    }
}
