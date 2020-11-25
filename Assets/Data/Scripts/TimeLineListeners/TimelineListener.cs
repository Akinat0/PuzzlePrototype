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
                case CutsceneMarker cutsceneMarker:
                    Debug.Log($"Notification received {time} type: {typeof(CutsceneMarker)} : {cutsceneMarker.SceneId}"); 
                    GameSceneManager.Instance.InvokeCutsceneStarted(new CutsceneEventArgs(cutsceneMarker.SceneId, cutsceneMarker.SceneTransitionType));
                    break;
                case LevelEndMarker levelEndMarker:
                    Debug.Log($"Notification received {time} type: {typeof(LevelEndMarker)}");
                    GameSceneManager.Instance.InvokeLevelCompleted();
                    break;
                case BubbleDialogMarker bubbleDialogMarker:
                    Debug.Log($"Notification received {time} type: {typeof(BubbleDialogMarker)}");
                    GameSceneManager.Instance.ShowDialog(bubbleDialogMarker.Message, bubbleDialogMarker.Time);
                    break;
            }
        }
    }

    protected virtual void OnEnable()
    {
        GameSceneManager.GameStartedEvent += GameStartedEvent_Handler;
        GameSceneManager.PauseLevelEvent += PauseLevelEvent_Handler;
        GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
    }

    protected virtual  void OnDisable()
    {
        GameSceneManager.GameStartedEvent -= GameStartedEvent_Handler;
        GameSceneManager.PauseLevelEvent -= PauseLevelEvent_Handler;
        GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
    }
    
    protected virtual void GameStartedEvent_Handler()
    {
        ReceiveNotifications = true;
        
        EditorStartLevel();
        
        PlayableDirector.Play();
    }

    protected virtual void PauseLevelEvent_Handler(bool paused)
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
        
        ReceiveNotifications = true;
    }
}
