
using System.Collections;
using Puzzle;
using PuzzleScripts;
using UnityEngine;

public class LongPuzzleEnemy : PuzzleEnemy
{
    [SerializeField] TrailRenderer Trail;

    float TrailTime;
        
    public override void Instantiate(EnemyParams @params)
    {
        base.Instantiate(@params);
        Trail.time = @params.trailTime;
        TrailTime = @params.trailTime;
    }
        
    public override void OnHitPlayer(Player player)
    {
        if (player.sides[side.GetHashCode()] != stickOut) //Sides shouldn't be equal
        {
            Die();
        } //That means everything's okay
        else
        {
            StartCoroutine(TrailRoutine(player));
        }
    }

    IEnumerator TrailRoutine(Player player)
    {
        float time = 0;
            
        while (time < TrailTime)
        {
            if (!IsValidPositions(player))
            {
                base.OnHitPlayer(player);
                yield break;
            }
    
            time += Time.deltaTime;
            yield return null;
        }
    
        Die();
    }

    bool IsValidPositions(Player player)
    {
        return (player.sides[(int) side] == stickOut);
    }

}
