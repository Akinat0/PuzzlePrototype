
public class InfinitySondTimeLineTimelineController : TimelineController
{
    protected override void StartIndex()
    {
        RandomIndex();
    }
    
    private void OnEnable()
    {
        InfinityGameSceneManager.ChangeSoundEvent += ChangeSound_Handler;
    }

    private void OnDisable()
    {
        InfinityGameSceneManager.ChangeSoundEvent -= ChangeSound_Handler;
    }

    private void ChangeSound_Handler()
    {
        NextTimeline();
    }
}
