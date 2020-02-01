using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PuzzleScripts;
using UnityEngine;

namespace Puzzle
{
    public class ShitEnemy : EnemyBase, ITouchProcessor
    {
        
        
        public void OnTouch(Touch touch)
        {
            Die();
        }
    }
}