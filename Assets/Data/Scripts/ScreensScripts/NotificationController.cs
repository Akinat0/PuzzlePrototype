using System.Collections.Generic;
using System.Linq;
using Puzzle;
using ScreensScripts;
using UnityEngine;

public class NotificationController : MonoBehaviour
{
    [SerializeField] AchievementNotification notification;
    
    readonly Queue<Achievement> achievements = new Queue<Achievement>();
    
    void Awake()
    {
        LauncherUI.GameEnvironmentUnloadedEvent += _ => TryDequeAchievement();
        LauncherUI.AchievementReceived += QueueAchievement;
    }

    void QueueAchievement(Achievement achievement)
    {
        achievements.Enqueue(achievement);

        bool notInGameNow = GameSceneManager.Instance == null;
        
        if(notInGameNow && achievements.Count == 1)
            TryDequeAchievement();
    }

    void TryDequeAchievement()
    {
        if(!achievements.Any())
            return;

        Achievement achievement = achievements.Dequeue();
        
        notification.Setup(achievement);
        
        notification.Show(TryDequeAchievement);
    }
}
