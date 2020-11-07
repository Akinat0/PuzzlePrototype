
using Puzzle;
using PuzzleScripts;

public sealed class FirstReviewAchievement : Achievement
{
    public FirstReviewAchievement()
    {
        if (State == AchievementState.Claimed || State == AchievementState.Received)
            return;
        
        GameSceneManager.PlayerReviveEvent += PlayerReviveEvent_Handler;
    }
    
    readonly CoinsReward reward = new CoinsReward(250);
    
    public override string Name => "New life";
    public override string Description => "Use Review";
    public override Reward Reward => reward;
    public override float Goal => 1;

    void PlayerReviveEvent_Handler()
    {
        Progress++;
    }

}
