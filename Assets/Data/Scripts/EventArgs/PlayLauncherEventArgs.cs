
public class PlayLauncherEventArgs
{
    private LevelConfig m_LevelConfig;


    public LevelConfig LevelConfig => m_LevelConfig;

    public PlayLauncherEventArgs(LevelConfig _LevelConfig)
    {
        m_LevelConfig = _LevelConfig;
    }
}