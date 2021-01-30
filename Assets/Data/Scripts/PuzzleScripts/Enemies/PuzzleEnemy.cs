using PuzzleScripts;
using UnityEngine;

namespace Puzzle
{
    public class PuzzleEnemy : EnemyBase
    {
        [HideInInspector] public Side side;
        [HideInInspector] public bool stickOut;

        public override void OnHitPlayer(Player player)
        {
            if (player.sides[(int)side] != stickOut) //Sides shouldn't be equal
            {
                Die();
            } //That means everything's okay
            else
            {
                base.OnHitPlayer(player);
            }
        }
        public override void Instantiate(EnemyParams @params)
        {
            side = @params.side;
            stickOut = @params.stickOut;
            base.Instantiate(@params);
        }

    }
}