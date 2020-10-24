using Puzzle;
using PuzzleScripts;
using UnityEngine;

public class RitualPuzzleEnemy : PuzzleEnemy
{
    [SerializeField] SkinContainer SkinContainer;
    [SerializeField] SpriteRenderer SpriteRenderer;

    public override Renderer Renderer => SpriteRenderer;

    public override void Instantiate(EnemyParams @params)
    {
        base.Instantiate(@params);
        
        //Sync child sprite renderer with world
        SpriteRenderer.transform.right = Vector3.right;
        
        SkinContainer.Skin = @params.stickOut ? 1 : 0;
    }
}
