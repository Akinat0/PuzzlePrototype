using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using Puzzle;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
public class TimelineListener : MonoBehaviour, INotificationReceiver
{
    protected PlayableDirector _playableDirector;
    protected double startTime = 0;
    
    protected virtual void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
    }

    bool ReceiveNotifications = false;

    public virtual void OnNotify(Playable origin, INotification notification, object context)
    {
        if (!ReceiveNotifications)
        {
            Debug.LogWarning($"Notification {notification.id} will be ignored", gameObject);
            return;
        }

        double time = origin.IsValid() ? origin.GetTime() : 0.0;
        if(notification is Marker marker && marker.time > startTime)
        {
            switch (notification)
            {
                case EnemyNotificationMarker enemyMarker:
                    Debug.Log($"Notification received {time} type: {enemyMarker.enemyParams.enemyType}");
                    GameSceneManager.Instance.InvokeCreateEnemy(enemyMarker.enemyParams);
                    break;
                case PlayAudioMarker audioMarker:
                    Debug.Log(
                        $"Notification received {time} type: {typeof(PlayAudioMarker)} : {audioMarker.AudioClip.name}");
                    GameSceneManager.Instance.InvokePlayAudio(new LevelPlayAudioEventArgs(audioMarker.AudioClip,
                        audioMarker.Looped, audioMarker.SoundCurve, 0));
                    break;
                case CutsceneMarker cutsceneMarker:
                    Debug.Log(
                        $"Notification received {time} type: {typeof(CutsceneMarker)} : {cutsceneMarker.SceneId}");
                    GameSceneManager.Instance.InvokeCutsceneStarted(new CutsceneEventArgs(cutsceneMarker.SceneId,
                        cutsceneMarker.SceneTransitionType));
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
        var timeLineAsset = (TimelineAsset) _playableDirector.playableAsset;
        startTime = 0;
        List<PlayAudioMarker> audioMarkers = new List<PlayAudioMarker>();
        for (int i=0; i < timeLineAsset.markerTrack.GetMarkerCount(); i++)
        {
            Debug.Log("Marker");
            IMarker marker = timeLineAsset.markerTrack.GetMarker(i);
            if (Mathf.Approximately((float)startTime, 0))
                if (marker is StartLevelMarker)
                    startTime = marker.time;
            if (marker is PlayAudioMarker playAudioMarker)
            {
                audioMarkers = audioMarkers.Append(playAudioMarker).ToList();
            }

            if (!Mathf.Approximately((float)startTime, 0))
            {
                Debug.Log("StartTime " + startTime);
                _playableDirector.time = startTime;
                foreach (var audioMarker in audioMarkers)
                {
                    if (audioMarker.time < startTime &&
                        audioMarker.time + audioMarker.AudioClip.length > startTime)
                    {
                        GameSceneManager.Instance.InvokePlayAudio(new LevelPlayAudioEventArgs(audioMarker.AudioClip,
                            audioMarker.Looped, audioMarker.SoundCurve, startTime-audioMarker.time));
                    }
                }
                break;
            }
        }
        _playableDirector.Play();
    }

    protected virtual void PauseLevelEvent_Handler(bool paused)
    {
        Pause(paused);
    }

    protected void Pause(bool paused)
    {
        if (!_playableDirector.playableGraph.IsValid())
            return;

        ReceiveNotifications = !paused;
        
        _playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(paused ? 0 : 1);
    }
    
    
    void ResetLevelEvent_Handler()
    {
        ReceiveNotifications = false;
        
        _playableDirector.Stop();
        _playableDirector.time = 0;
        
        ReceiveNotifications = true;
    }
}
