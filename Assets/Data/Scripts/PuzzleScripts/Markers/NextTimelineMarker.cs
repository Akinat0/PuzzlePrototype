using System.Collections;
using System.Collections.Generic;
using PuzzleScripts;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class NextTimelineMarker : Marker, INotification
{
    [SerializeField] public TimelinePlayable nextTimeline;
    public PropertyName id { get; }
}
