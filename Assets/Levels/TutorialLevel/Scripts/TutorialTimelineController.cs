public class TutorialTimelineController : TimelineController
{
    private void OnEnable()
    {
        TutorialManager.OnTutorialNextStage += OnTutorialNextStage_Handler;
    }

    private void OnDisable()
    {
        TutorialManager.OnTutorialNextStage -= OnTutorialNextStage_Handler;
    }
    
    void OnTutorialNextStage_Handler()
    {
        NextIndex();
    }
}
