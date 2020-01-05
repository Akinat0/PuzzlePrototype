using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

//Kind of crutch
[Serializable]
public class LevelEndMarker : Marker, INotification
{
    public PropertyName id { get; }
}


