
public class TutorialAchievement : Achievement
{
    readonly CoinsReward reward = new CoinsReward(230);
    
    public override string Name => "Start Learning";
    public override string Description => "Complete tutorial";
    public override Reward Reward => reward;
    public override float Goal => 1;
    
}
