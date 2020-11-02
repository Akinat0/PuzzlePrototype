
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
    
    public override int ID => 2;
    public override Reward Reward => new PuzzleReward(Puzzle.ID);
    public override Purchase Purchase => new CoinsPurchase(1000);
    
    void OnBalanceChangedEvent_Handler(int balance)
    {
        Available = Purchase.Available && !Puzzle.Unlocked;
    }

    void OnUnlockedEvent_Handler(bool unlocked)
    {
        Available = Purchase.Available && !Puzzle.Unlocked;
    }
}
