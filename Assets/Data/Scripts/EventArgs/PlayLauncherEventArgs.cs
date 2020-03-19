
public class PlayLauncherEventArgs
{
    private LevelConfig m_LevelConfig;

    public LevelConfig LevelConfig => m_LevelConfig;

    public PlayLauncherEventArgs(LevelConfig _LevelConfig)
    {
        m_LevelConfig = _LevelConfig;
    }
}

public class CloseCollectionEventArgs
{
    private PlayerView m_PlayerView;

    public PlayerView PlayerView => m_PlayerView;

    public CloseCollectionEventArgs(PlayerView _PlayerView = null)
    {
        m_PlayerView = _PlayerView;
    }
}