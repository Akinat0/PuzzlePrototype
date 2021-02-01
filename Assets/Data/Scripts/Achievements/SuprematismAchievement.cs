public sealed class SuprematismAchievement : Achievement
{
    public SuprematismAchievement()
    {
        if (State == AchievementState.Claimed || State == AchievementState.Received)
            return;
        
        LevelConfig suprematism = Account.GetLevel("Suprem");
        
        if (suprematism.StarsAmount >= 3)
        { 
            Progress = Goal;
            return;
        }
            
        suprematism.StarsAmountChanged += LevelStarsAmountChanged_Handler;
    }

    readonly PuzzleReward reward = new PuzzleReward(Account.GetCollectionItem("Malevich").ID);

    public override string Name => "Suprematism";
    public override string Description => "Complete suprematism on 3 stars";
    public override Reward Reward => reward;
    public override float Goal => 1;
    
    void LevelStarsAmountChanged_Handler(int starsAmount)
    {
        if (starsAmount >= 3)
            Progress = Goal;
    }
}
