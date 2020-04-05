using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[CustomStyle("LevelEndMarker"), Serializable]
public class LevelEndMarker : Marker, INotification
{
    public PropertyName id { get; }
    
}


