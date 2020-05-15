﻿using System.Linq;
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
    
    protected virtual void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
    }

    public virtual void OnNotify(Playable origin, INotification notification, object context)
    {
        double time = origin.IsValid() ? origin.GetTime() : 0.0;

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
        
        _playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(paused ? 0 : 1);
    }
    
    
    private void ResetLevelEvent_Handler()
    {
        _playableDirector.Stop();
        _playableDirector.time = 0;
    }
}
