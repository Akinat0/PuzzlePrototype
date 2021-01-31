using PuzzleScripts;
using UnityEngine;

namespace Puzzle
{
    public class ShitEnemy : EnemyBase, ITouchProcessor
    {
        public void OnTouch()
        {
            Die();
        }
    }
}