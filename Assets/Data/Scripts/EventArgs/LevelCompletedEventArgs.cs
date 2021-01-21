
public class LevelCompletedEventArgs
{
    public LevelConfig LevelConfig { get; private set; }
    public int LivesLeft { get; private set; }
    public Booster[] BoostersUsed { get; private set; }
    public bool ReviveUsed { get; private set; }

    public LevelCompletedEventArgs(LevelConfig levelConfig, int livesLeft, Booster[] boostersUsed, bool reviveUsed)
    {
        LevelConfig = levelConfig;
        LivesLeft = livesLeft;
        BoostersUsed = boostersUsed;
        ReviveUsed = reviveUsed;
    }    
}
