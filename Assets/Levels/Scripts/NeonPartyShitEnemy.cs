using Puzzle;
using PuzzleScripts;
using UnityEngine;

public class NeonPartyShitEnemy : ShitEnemy
{
    
    [SerializeField] private SpriteRenderer ChildSpriteRenderer;

    public override void SetCoinHolder(int CostOfEnemy)
    {
        NeonPartyShitCoinHolder coinHolder = gameObject.AddComponent<NeonPartyShitCoinHolder>();
        
        coinHolder.spriteRenderer = ChildSpriteRenderer;
        coinHolder.SetupCoinHolder(CostOfEnemy);
    }
}