using System.Collections;
using System.Collections.Generic;
using Puzzle;
using PuzzleScripts;
using UnityEngine;

public class ParentsPuzzle : PuzzleEnemy
{
    public override void Instantiate(EnemyParams @params)
    {
        base.Instantiate(@params);
        GetComponent<SkinContainer>().Skin = stickOut ? 1 : 0;
        GetComponent<SpriteRenderer>().flipX = true;
    }
}
