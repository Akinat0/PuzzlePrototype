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
            if (!CanDamagePlayer(player))
                Die();
            else
                base.OnHitPlayer(player);
        }

        public override bool CanDamagePlayer(Player player) => base.CanDamagePlayer(player) && (player.sides[(int)side] == stickOut);

        public override void Instantiate(EnemyParams @params)
        {
            side = @params.side;
            stickOut = @params.stickOut;
            base.Instantiate(@params);
        }
        
    }
}