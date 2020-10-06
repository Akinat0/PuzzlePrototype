using System;
using Puzzle;
using PuzzleScripts;
using UnityEngine;

[RequireComponent(typeof(SkinContainer))]
public class RitualLongPuzzleEnemy : LongPuzzleEnemy
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

        int skinIndex = 0;
        
        switch (@params.side)
        {
            case Side.Right:
                skinIndex = 0;
                break;
            case Side.Left:
                skinIndex = 1;
                break;
            case Side.Down:
                skinIndex = 2;
                break;
            case Side.Up:
                skinIndex = 3;
                break;
        }
  
        if (!@params.stickOut)
            skinIndex += 4;
        
        SkinContainer.Skin = skinIndex;
        
        if(@params.side == Side.Right)
            transform.Rotate(new Vector3(180, 0, 0));
    }
}
