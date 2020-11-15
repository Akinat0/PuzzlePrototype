
using Puzzle;

public class DifficultyTimelineListener : TimelineListener
{
    protected override void OnEnable()
    {
        base.OnEnable();
        GameSceneManager.SetupLevelEvent += SetupLevelEvent_Handler;
    }  
    
    protected override void OnDisable()
    {
        base.OnDisable();
        GameSceneManager.SetupLevelEvent -= SetupLevelEvent_Handler;
    }
    
    void SetupLevelEvent_Handler(LevelColorScheme _)
    {
        LevelConfig config = GameSceneManager.Instance.LevelConfig;
        
        if(config.DifficultyLevel == DifficultyLevel.Invalid)
            return;

        PlayableDirector.playableAsset = config.GetTimeline(config.DifficultyLevel);
    }
}
