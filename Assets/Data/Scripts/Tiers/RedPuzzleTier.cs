
public sealed class RedPuzzleTier : Tier
{
    readonly CollectionItem Puzzle;
    
    public RedPuzzleTier()
    {
        Puzzle = Account.GetCollectionItem("Red");
        
        Available = Purchase.Available && !Puzzle.Unlocked;
        
        Account.BalanceChangedEvent += OnBalanceChangedEvent_Handler;
        Puzzle.OnUnlockedEvent += OnUnlockedEvent_Handler;
    }
    
    public override int ID => 3;
    public override Reward Reward => new PuzzleReward(Puzzle.ID);
    public override Purchase Purchase => new CoinsPurchase(1000);
    public override TierType Type => TierType.Puzzle;

    void OnBalanceChangedEvent_Handler(int balance)
    {
        Available = Purchase.Available && !Puzzle.Unlocked;
    }

    void OnUnlockedEvent_Handler(bool unlocked)
    {
        Available = Purchase.Available && !Puzzle.Unlocked;
    }
}
