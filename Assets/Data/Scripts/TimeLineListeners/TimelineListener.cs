using Puzzle;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
public partial class TimelineListener : MonoBehaviour, INotificationReceiver
{
    PlayableDirector playableDirector;
   
    protected PlayableDirector PlayableDirector
    {
        get
        {
            if (playableDirector == null)
                playableDirector = GetComponent<PlayableDirector>();

            return playableDirector;
        }
    }
    
    protected virtual void Awake() { }

    protected double startTime = 0;
    
    bool ReceiveNotifications = false;

    public virtual void OnNotify(Playable origin, INotification notification, object context)
    {
        if (!ReceiveNotifications)
        {
            Debug.LogWarning($"Notification {notification.id} will be ignored", gameObject);
            return;
        }

        double time = origin.IsValid() ? origin.GetTime() : 0.0;
        if(notification is Marker marker && marker.time >= startTime)
        {
            switch (notification)
            {
                case EnemyNotificationMarker enemyMarker:
                    Debug.Log($"Notification received {time} type: {enemyMarker.enemyParams.enemyType}");
                    GameSceneManager.Instance.InvokeCreateEnemy(enemyMarker.enemyParams);
                    break;
                case PlayAudioMarker audioMarker:
                    Debug.Log($"Notification received {time} type: {typeof(PlayAudioMarker)} : {audioMarker.AudioClip.name}");
                    GameSceneManager.Instance.InvokePlayAudio(new LevelPlayAudioEventArgs(audioMarker.AudioClip, audioMarker.Looped, audioMarker.SoundCurve));
                    break;
                case LevelEndMarker _:
                    Debug.Log($"Notification received {time} type: {typeof(LevelEndMarker)}");
                    GameSceneManager.Instance.InvokeLevelCompleted();
                    break;
                case FirstStarMarker _:
                    Debug.Log($"Notification received {time} type: {typeof(FirstStarMarker)}");
                    GameSceneManager.Instance.LevelConfig.ObtainFirstStar();
                    break;
                case EventMarker eventMarker:
                    Debug.Log($"Notification received {time} type: {typeof(EventMarker)} : {eventMarker.EventData}");
                    GameSceneManager.Instance.InvokeTimelineEvent(eventMarker.EventData);
                    break;
            }
        }
    }

    protected virtual void OnEnable()
    {
        GameSceneManager.PauseLevelEvent += PauseLevelEvent_Handler;
        GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
        GameSceneManager.LevelClosedEvent += LevelClosedEvent_Handler;
    }

    protected virtual void OnDisable()
    {
        GameSceneManager.PauseLevelEvent -= PauseLevelEvent_Handler;
        GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
        GameSceneManager.LevelClosedEvent -= LevelClosedEvent_Handler;
    }
    

    protected void PauseLevelEvent_Handler(bool paused)
    {
        Pause(paused);
    }

    protected void Pause(bool paused)
    {
        if (!PlayableDirector.playableGraph.IsValid())
            return;

        ReceiveNotifications = !paused;
        
        PlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(paused ? 0 : 1);
    }
    
    
    void ResetLevelEvent_Handler()
    {
        ReceiveNotifications = false;
        
        PlayableDirector.Stop();
        PlayableDirector.time = 0;
        
        EditorStartLevel();
        
        ReceiveNotifications = true;
        
        PlayableDirector.Play();
    }

    void LevelClosedEvent_Handler()
    {
        ReceiveNotifications = false;
        PlayableDirector.Stop();
    }
}
