using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[CustomStyle("EventMarker")]
public class EventMarker : Marker, INotification
{
    [SerializeField] private string eventData;

    public string EventData => eventData;

    public PropertyName id { get; }
}
