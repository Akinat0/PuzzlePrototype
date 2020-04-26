using Puzzle;

public class TutorialTimelineController : TimelineController
{
    private void OnEnable()
    {
        TutoriaScenelManager.OnTutorialNextStage += OnTutorialNextStage_Handler;
        TutoriaScenelManager.OnTutorialRestart += OnTutorialRestart_Handler;
        GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
    }

    private void OnDisable()
    {
        TutoriaScenelManager.OnTutorialNextStage -= OnTutorialNextStage_Handler;
        TutoriaScenelManager.OnTutorialRestart -= OnTutorialRestart_Handler;
        GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
    }
    
    void OnTutorialNextStage_Handler()
    {
        NextTimeline();
    }
    
    void OnTutorialRestart_Handler()
    {
        Refresh();
    }
    
    void ResetLevelEvent_Handler()
    {
        StartIndex();
        Refresh();
    }
}
