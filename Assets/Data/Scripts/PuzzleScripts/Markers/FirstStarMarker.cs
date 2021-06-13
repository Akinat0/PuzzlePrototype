using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[CustomStyle("FirstStarMarker"), Serializable]
public class FirstStarMarker : Marker, INotification
{
    public PropertyName id { get; }
}
