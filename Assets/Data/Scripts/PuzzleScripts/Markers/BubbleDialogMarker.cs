using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[CustomStyle("BubbleDialogMarker")]
public class BubbleDialogMarker : Marker, INotification
{
    [SerializeField] private string message;
    [SerializeField] private float time;

    public string Message => message;
    public float Time => time;

    public PropertyName id { get; }
    
}
