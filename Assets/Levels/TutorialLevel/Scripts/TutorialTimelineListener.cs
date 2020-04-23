using Puzzle;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialTimelineListener : TimelineListener
{
    public override void OnNotify(Playable origin, INotification notification, object context)
    {
        base.OnNotify(origin, notification, context);

        if (GameSceneManager.Instance is TutorialManager manager)
        {
            switch (notification)
            {
                case GotoMarker gotoMarker:

                    manager.InvokeTutorialNextStage();
                    break;

                case TutorGiveControlMarker giveControlMarker:
                    manager.InvokeEnableInput();
                    break;
                
                case TutorEndFirstStageMarker endFirstStageMarker:
                    //TODO
                    break;
            }
        }
        else
        {
            Debug.LogError("Tutorial scene manager didn't found");
        }
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        TutorialManager.OnStopTutorial += OnStopTutorial_Handler;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        TutorialManager.OnStopTutorial -= OnStopTutorial_Handler;
    }

    void OnStopTutorial_Handler(bool paused)
    {
        if (!_playableDirector.playableGraph.IsValid()) 
            return;
        _playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(paused ? 0 : 1);
    }

    protected override void PauseLevelEvent_Handler(bool paused)
    {
        if (!paused)
        {
            if (!TutorialManager.TutorialStopped)
                base.PauseLevelEvent_Handler(paused);
        }
        else
        {
            base.PauseLevelEvent_Handler(paused);
        }
    }
}
