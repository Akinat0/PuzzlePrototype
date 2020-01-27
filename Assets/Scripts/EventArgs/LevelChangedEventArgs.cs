
public class LevelChangedEventArgs
{
    private PlayerView m_PlayerView;
    private LevelConfig m_LevelConfig;

    public PlayerView PlayerView => m_PlayerView;
    public LevelConfig LevelConfig => m_LevelConfig;

    public LevelChangedEventArgs(PlayerView _PlayerView, LevelConfig _LevelConfig)
    {
        m_PlayerView = _PlayerView;
        m_LevelConfig = _LevelConfig;
    }
}
