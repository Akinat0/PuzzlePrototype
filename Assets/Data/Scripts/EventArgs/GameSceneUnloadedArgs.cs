
public class GameSceneUnloadedArgs
{

    public readonly GameSceneUnloadedReason Reason;
    public readonly bool ShowStars;
    public readonly LevelConfig LevelConfig;
    
    public enum GameSceneUnloadedReason
    {
        LevelCompleted,
        LevelClosed
    }
    public GameSceneUnloadedArgs(GameSceneUnloadedReason reason, bool showStars, LevelConfig levelConfig)
    {
        Reason = reason;
        ShowStars = showStars;
        LevelConfig = levelConfig;
    }    
}
