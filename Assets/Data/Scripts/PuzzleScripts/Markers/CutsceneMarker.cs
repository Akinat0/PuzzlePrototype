using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[CustomStyle("CutsceneMarker")]
public class CutsceneMarker : Marker, INotification
{
    [SerializeField] private string sceneId;
    [SerializeField] private SceneTransitionType sceneTransitionType;
    
    public string SceneId => sceneId;
    public SceneTransitionType SceneTransitionType => sceneTransitionType;

    public PropertyName id { get; }
    
}
