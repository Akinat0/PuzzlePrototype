
public class GameSceneUnloadedArgs
{

    public readonly GameSceneUnloadedReason Reason;
    public enum GameSceneUnloadedReason
    {
        LevelCompleted,
        LevelClosed
    }
    public GameSceneUnloadedArgs(GameSceneUnloadedReason reason)
    {
        Reason = reason;
    }    
}
