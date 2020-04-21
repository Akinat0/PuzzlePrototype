using UnityEngine;
using UnityEngine.Playables;

public class TutorialTimelineListener : TimelineListener
{

    [SerializeField] private TutorialManager _manager;

    public override void OnNotify(Playable origin, INotification notification, object context)
    {
        base.OnNotify(origin, notification, context);
        if(notification is GotoMarker)
        {
            _manager.GoToNext();
        }

        if (notification is TutorGiveControlMarker)
            _manager.InvokeEnableInput();

        if (notification is TutorEndFirstStageMarker)
        { 
            //TODO
        }
    }

    protected override void GameStartedEvent_Handler()
    {
        _manager.StartFirstDirector();
    }
    

}
