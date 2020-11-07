
public sealed class GreenPuzzleTier : Tier
{
    readonly CollectionItem Puzzle;
    
    public GreenPuzzleTier()
    {
        Puzzle = Account.GetCollectionItem("Green");
        
        Available = Purchase.Available && !Puzzle.Unlocked;
        
        Account.BalanceChangedEvent += OnBalanceChangedEvent_Handler;
        Puzzle.OnUnlockedEvent += OnUnlockedEvent_Handler;
    }

    CoinsPurchase purchase = new CoinsPurchase(1000);
    
    public override int ID => 2;
    public override Reward Reward => new PuzzleReward(Puzzle.ID);
    public override Purchase Purchase => purchase;
    public override TierType Type => TierType.Puzzle;
    
    public override void Parse(TierInfo tierInfo)
    {
        if (tierInfo == null)
            return;
        
        purchase = new CoinsPurchase(tierInfo.Cost);
        InvokeTierValueChanged();
    }

    void OnBalanceChangedEvent_Handler(int balance)
    {
        Available = Purchase.Available && !Puzzle.Unlocked;
    }

    void OnUnlockedEvent_Handler(bool unlocked)
    {
        Available = Purchase.Available && !Puzzle.Unlocked;
    }
}
