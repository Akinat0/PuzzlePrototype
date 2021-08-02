
using Puzzle;
using PuzzleScripts;

public sealed class PuzzleCatcherAchievement : Achievement
{
    public PuzzleCatcherAchievement()
    {
        if (State == AchievementState.Claimed || State == AchievementState.Received)
            return;
        
        GameSceneManager.EnemyDiedEvent += EnemyDiedEvent_Handler;
    }
    
    readonly ChestReward reward = new ChestReward(Rarity.Rare, 1);
    
    public override string Name => "Puzzle Catcher";
    public override string Description => "Catch 1000 puzzle";
    public override Reward Reward => reward;
    public override float Goal => 1000;

    void EnemyDiedEvent_Handler(EnemyBase enemyBase)
    {
        if (enemyBase.Type == EnemyType.Puzzle ||
            enemyBase.Type == EnemyType.LongPuzzle)
        {
            Progress++;
        }
    }

}
