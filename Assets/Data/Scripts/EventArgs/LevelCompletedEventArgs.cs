
public class LevelCompletedEventArgs
{
    public LevelConfig LevelConfig { get; private set; }

    public LevelCompletedEventArgs(LevelConfig levelConfig)
    {
        LevelConfig = levelConfig;
    }    
}
