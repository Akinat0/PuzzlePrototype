﻿using Puzzle;
using PuzzleScripts;

public class NeonParatyEnemy : PuzzleEnemy
{
    public override void Instantiate(EnemyParams @params)
    {
        base.Instantiate(@params);
        GetComponent<SkinContainer>().Skin = @params.stickOut ? 0 : 1;
    }
}
