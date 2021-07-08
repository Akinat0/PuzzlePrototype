using System;
using Abu.Tools.UI;
using Puzzle;

public class PauseButtonComponent : ButtonComponent
{
    void Start()
    {
        if(GameSceneManager.Instance != null)
            GameSceneManager.Instance.LevelConfig.ColorScheme.SetButtonColor(this);
        
        GameSceneManager.PauseLevelEvent += OnPauseLevelEvent_Handler;
        GameSceneManager.LevelCompletedEvent += LevelCompletedEvent_Handler;
        OnClick += () => GameSceneManager.Instance.InvokePauseLevel(true, false);
    }

    void OnDestroy()
    {
        GameSceneManager.PauseLevelEvent -= OnPauseLevelEvent_Handler;
        GameSceneManager.LevelCompletedEvent -= LevelCompletedEvent_Handler;
    }

    void OnPauseLevelEvent_Handler(bool pause)
    {
        if(pause)
            HideComponent();
        else
            ShowComponent();
    }

    void LevelCompletedEvent_Handler(LevelCompletedEventArgs _)
    {
        HideComponent();
    }
}
