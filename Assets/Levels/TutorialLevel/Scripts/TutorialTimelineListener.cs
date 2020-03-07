﻿using Puzzle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialTimelineListener : TimelineListener
{

    [SerializeField] private TutorialTimelineManager _manager;

    public override void OnNotify(Playable origin, INotification notification, object context)
    {
        base.OnNotify(origin, notification, context);
        if(notification is GotoMarker)
        {
            _manager.GoToNext();
        }
    }

    protected override void GameStartedEvent_Handler()
    {
        _manager.StartFirstDirector();
    }


}
