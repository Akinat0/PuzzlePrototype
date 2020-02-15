using System.Collections;
using System.Collections.Generic;
using Puzzle;
using PuzzleScripts;
using UnityEngine;

public class NeonPuzzle : PuzzleEnemy
{
    public override void Instantiate(EnemyParams @params)
    {
        base.Instantiate(@params);
        GetComponent<SpriteRenderer>().flipX = !@params.stickOut;
    }
}
