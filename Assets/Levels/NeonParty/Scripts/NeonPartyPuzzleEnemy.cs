using Puzzle;
using PuzzleScripts;
using UnityEngine;

public class NeonPartyPuzzleEnemy : PuzzleEnemy
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
    
    public override void Instantiate(EnemyParams @params)
    {
        base.Instantiate(@params);
        SkinContainer.Skin = @params.stickOut ? 1 : 0;
        
        if(@params.side == Side.Right)
            transform.Rotate(new Vector3(180, 0, 0));
    }
}