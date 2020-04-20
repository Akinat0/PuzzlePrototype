
public class InfinityPatternTimeLineTimelineController : TimelineController
{
    private void OnEnable()
    {
        InfinityGameSceneManager.ChangePatternEvent += ChangePattern_Handler;
    }

    private void OnDisable()
    {
        InfinityGameSceneManager.ChangePatternEvent -= ChangePattern_Handler;
    }

    private void ChangePattern_Handler()
    {
        RandomIndex();
        NextTimeLine();
    }
}
