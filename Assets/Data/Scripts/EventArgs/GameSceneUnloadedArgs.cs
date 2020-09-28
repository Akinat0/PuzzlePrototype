
public class GameSceneUnloadedArgs
{

    public readonly GameSceneUnloadedReason Reason;
    public readonly bool ShowStars;
    
    public enum GameSceneUnloadedReason
    {
        LevelCompleted,
        LevelClosed
    }
    public GameSceneUnloadedArgs(GameSceneUnloadedReason reason, bool showStars)
    {
        Reason = reason;
        ShowStars = showStars;
    }    
}
