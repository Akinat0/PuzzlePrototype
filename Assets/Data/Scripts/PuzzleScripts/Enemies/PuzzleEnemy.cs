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
            if (player.sides[side.GetHashCode()] != stickOut) //Sides shouldn't be equal
            {
                Die();
            } //That means everything's okay
            else
            {
                base.OnHitPlayer(player);
            }
        }
        public override void Instantiate(EnemyParams @params)//Side side, float? speed = null)
        {
            side = @params.side;
            stickOut = @params.stickOut;
            base.Instantiate(@params);
            GetComponent<SpriteRenderer>().sprite = stickOut ? GetComponent<SkinContainer>().sprites [0]: GetComponent<SkinContainer>().sprites[1];

            //if (!stickOut)
            GetComponent<SpriteRenderer>().flipX = true;
        }
        
    }
}