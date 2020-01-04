using Puzzle;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineListener : MonoBehaviour, INotificationReceiver
{
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        double time = origin.IsValid() ? origin.GetTime() : 0.0;
        
        EnemyNotificationMarker enemyMarker = notification as EnemyNotificationMarker;
        if (enemyMarker != null)
        {
            Debug.Log("Notification received " + time + " type: " + enemyMarker.enemyParams.enemyType);
            GameSceneManager.Instance.InvokeCreateEnemy(enemyMarker.enemyParams);
        }
    }
}
