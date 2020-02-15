using System;
using Puzzle;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineListener : MonoBehaviour, INotificationReceiver
{
    private PlayableDirector _playableDirector;
    
    private void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
    }

    public void OnNotify(Playable origin, INotification notification, object context)
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
            GameSceneManager.Instance.InvokeLevelClosed();
            return;
        }
    }

    private void OnEnable()
    {
        GameSceneManager.PauseLevelEvent += PauseLevelEvent_Handler;
        GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
    }

    private void OnDisable()
    {
        GameSceneManager.PauseLevelEvent -= PauseLevelEvent_Handler;
        GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
    }

    private void PauseLevelEvent_Handler(bool paused)
    {
        if (paused && _playableDirector.state != PlayState.Paused)
            _playableDirector.Pause();

        if (!paused && _playableDirector.state == PlayState.Paused)
            _playableDirector.Play();
    }
    
    private void ResetLevelEvent_Handler()
    {
        _playableDirector.Stop();
        _playableDirector.time = 0;
       // _playableDirector.Play();
    }
}
