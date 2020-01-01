using PuzzleScripts;
using UnityEngine;

namespace Puzzle
{
    public class PuzzleEnemy : EnemyBase
    {
        [HideInInspector]
        public Side side;
        
        public override void OnHitPlayer(Player player)
        {
            if (!player.sides[side.GetHashCode()])
            {
                Die();
            } //That means everything's okay
            else
            {
                base.OnHitPlayer(player);
            }
        }
        public override void Instantiate(Side side, float? speed = null)
        {
            this.side = side;
            base.Instantiate(side, speed);
            GetComponent<SpriteRenderer>().color = UnityEngine.Random.ColorHSV();
        }
        
    }
}