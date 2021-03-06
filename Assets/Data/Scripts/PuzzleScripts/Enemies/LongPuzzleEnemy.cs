using System.Collections;
using Puzzle;
using PuzzleScripts;
using UnityEngine;

public class LongPuzzleEnemy : PuzzleEnemy
{
    [SerializeField] DetachedVFX CollisionFX;
    [SerializeField] TrailRenderer Trail;

    float TrailTime;

    DetachedVFX effect;
        
    public override void Instantiate(EnemyParams @params)
    {
        base.Instantiate(@params);
        Trail.time = @params.trailTime;
        TrailTime = @params.trailTime;
        Trail.widthMultiplier *= transform.lossyScale.x;
        Trail.sortingLayerName = RenderLayer.Player;
        Trail.sortingOrder = -1;
    }
        
    public override void OnHitPlayer(Player player)
    {
        Collider.enabled = false;
        
        if (!CanDamagePlayer(player))
        {
            Motion = false;
            transform.position = GetTargetPosition(player);
            Renderer.enabled = false;

            effect = Instantiate(CollisionFX, GameSceneManager.Instance.GameSceneRoot);
            effect.Initialize(transform, GameSceneManager.Instance.GameSceneRoot);
            effect.Play();
            
            StartCoroutine(TrailRoutine(player));
        }
        else
        {
            base.OnHitPlayer(player);
        }
    }

    public override Transform Die()
    {
        Account.AddCoins(1);
        return base.Die();
    }

    public override void SetCoinHolder(int CostOfEnemy)
    {
        gameObject.AddComponent<EmptyCoinHolder>().SetupCoinHolder(CostOfEnemy);
    }

    Vector3 GetTargetPosition(Player player)
    {
        Vector3 targetPosition = player.PlayerView.GetSidePosition(side);

        if (side == Side.Left || side == Side.Right)
            targetPosition.x -= (targetPosition.x - player.transform.position.x) / 2;  

        if (side == Side.Up || side == Side.Down)
            targetPosition.y -= (targetPosition.y - player.transform.position.y) / 2;
        
        return targetPosition;
    }
    
    IEnumerator TrailRoutine(Player player)
    {
        float time = 0;

        while (time < TrailTime)
        {
            if (!IsValidPositions(player))
            {
                Destroy(gameObject);
                yield break;
            }
    
            time += Time.deltaTime;
            yield return null;
        }
        
        Die();
    }

    bool IsValidPositions(Player player)
    {
        return player.sides[(int) side] != stickOut;
    }

    void OnDestroy()
    {
        if(effect != null)
            effect.Hide();
    }
}
