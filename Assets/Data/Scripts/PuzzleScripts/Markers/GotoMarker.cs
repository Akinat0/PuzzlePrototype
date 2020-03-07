using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class GotoMarker : Marker, INotification
{
    public PropertyName id { get; }
}
