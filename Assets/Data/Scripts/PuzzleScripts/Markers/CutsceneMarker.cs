using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[CustomStyle("CutsceneMarker")]
public class CutsceneMarker : Marker, INotification
{
    [SerializeField] private string sceneId;
    
    public string SceneId => sceneId;

    public PropertyName id { get; }
    
}
