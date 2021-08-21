
public class PlayLauncherEventArgs
{
    public LevelConfig LevelConfig { get; }

    public LevelRootView LevelRootView { get; }

    public PlayLauncherEventArgs(LevelConfig _LevelConfig, LevelRootView _LevelRootView)
    {
        LevelConfig = _LevelConfig;
        LevelRootView = _LevelRootView;
    }
}

public class CloseCollectionEventArgs
{
    public PlayerView PlayerView { get; }

    public CloseCollectionEventArgs(PlayerView _PlayerView = null)
    {
        PlayerView = _PlayerView;
    }
}