
public class InfinitySondTimeLineController : Controller
{
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
        NextTimeLine();
    }
}
