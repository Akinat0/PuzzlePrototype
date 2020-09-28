
public class PlayLauncherEventArgs
{
    private readonly LevelConfig m_LevelConfig;
    private readonly LevelRootView m_LevelRootView;

    public LevelConfig LevelConfig => m_LevelConfig;
    public LevelRootView LevelRootView => m_LevelRootView;

    public PlayLauncherEventArgs(LevelConfig _LevelConfig, LevelRootView _LevelRootView)
    {
        m_LevelConfig = _LevelConfig;
        m_LevelRootView = _LevelRootView;
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