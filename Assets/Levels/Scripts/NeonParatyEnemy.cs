using Puzzle;
using PuzzleScripts;
using UnityEngine;

public class NeonParatyEnemy : PuzzleEnemy
{
    public override void Instantiate(EnemyParams @params)
    {
        base.Instantiate(@params);
        GetComponent<SkinContainer>().Skin = @params.stickOut ? 0 : 1;
        
        if(@params.side == Side.Right)
            transform.Rotate(new Vector3(180, 0, 0));
    }
}