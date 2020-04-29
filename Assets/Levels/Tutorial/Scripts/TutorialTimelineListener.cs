﻿using Puzzle;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialTimelineListener : TimelineListener
{
    public override void OnNotify(Playable origin, INotification notification, object context)
    {
        base.OnNotify(origin, notification, context);

        if (GameSceneManager.Instance is TutoriaScenelManager manager)
        {
            switch (notification)
            {
                case GotoMarker gotoMarker:

                    manager.InvokeTutorialNextStage();
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
        TutoriaScenelManager.OnStopTutorial += OnStopTutorial_Handler;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        TutoriaScenelManager.OnStopTutorial -= OnStopTutorial_Handler;
    }

    void OnStopTutorial_Handler(bool paused)
    {
        Pause(paused);
    }

    protected override void PauseLevelEvent_Handler(bool paused)
    {
        if (!paused)
        {
            if (!TutoriaScenelManager.TutorialStopped)
                base.PauseLevelEvent_Handler(paused);
        }
        else
        {
            base.PauseLevelEvent_Handler(paused);
        }
    }
}