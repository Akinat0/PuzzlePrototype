using UnityEngine;
using UnityEngine.Timeline;

[CustomStyle("NoteMarker")]
public class NoteMarker : Marker
{
    [SerializeField] private string note;

    public PropertyName id { get; }
    
}