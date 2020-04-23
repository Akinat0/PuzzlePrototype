﻿using System;
using Puzzle;
using UnityEngine;
using UnityEngine.Playables;

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

        if (notification is EnemyNotificationMarker enemyMarker)
        {
            Debug.Log("Notification received " + time + " type: " + enemyMarker.enemyParams.enemyType);
            GameSceneManager.Instance.InvokeCreateEnemy(enemyMarker.enemyParams);
            return;
        }
        
        if (notification is LevelEndMarker)
        {
            Debug.Log("Notification received " + time + " type: level end marker");
            GameSceneManager.Instance.InvokeLevelCompleted();
            return;
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

    private void PauseLevelEvent_Handler(bool paused)
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