
using System;
using System.Collections.Generic;
using Puzzle.Analytics;
using ScreensScripts;
using UnityEngine;

public abstract class Achievement
{
    public enum AchievementState
    {
        InProgress = 0,
        Received = 1, 
        Claimed = 2
    }
    
    public event Action<float> ProgressChangedEvent;
    public event Action AchievementReceivedEvent;
    public event Action AchievementClaimedEvent; 
    
    public static Achievement[] CreateAllAchievements()
    {
        List<Achievement> achievements = new List<Achievement>
        {
            new TutorialAchievement(),
            new RichBitchAchievement(),
            new FreezeBoosterAchievement(),
            new SuprematismAchievement(),
            new RitualAchievement(),
            new NeonPartyAchievement(),
            new PuzzleCatcherAchievement(),
            new FirstReviveAchievement(),
            new HeartBoosterAchievement(),
        };
        return achievements.ToArray();
    }

    protected Achievement()
    {
        int stateCode = PlayerPrefs.GetInt(StateKey, 0);
        
        State = (AchievementState)stateCode;

        Progress = PlayerPrefs.GetFloat(ProgressKey, 0.0f);

        AchievementReceivedEvent += () => LauncherUI.Instance.InvokeAchievementReceived(this);
    }

    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract Reward Reward { get; }
    public abstract float Goal { get; }
    public virtual Sprite Icon => Resources.Load<Sprite>("Achievements/DefaultAchievement");

    string Key => Name;
    string ProgressKey => Key + "Progress";
    string StateKey => Key + "State";

    AchievementState state;

    public AchievementState State
    {
        get => state;
        protected set
        {
            state = value;
            SaveState();
        }
    }

    float progress;

    public float Progress
    {
        get => progress;

        set
        {
            if (State == AchievementState.Received || State == AchievementState.Claimed)
            {
                progress = Goal;
                return;
            }
            
            progress = Mathf.Clamp(value, 0, Goal);
            
            SaveProgress();
            
            if (Progress >= Goal || Mathf.Approximately(Progress, Goal))
                ReceiveAchievement();
            
            ProgressChangedEvent?.Invoke(progress);
        }
    }

    public void Claim()
    {
        if (State != AchievementState.Received)
            return;
        
        State = AchievementState.Claimed;
        Reward.Claim();
        AchievementClaimedEvent?.Invoke();
        
        new SimpleAnalyticsEvent("achievement_claimed", GetAnalyticsData()).Send();
    }
    

    protected virtual void ReceiveAchievement()
    {
        State = AchievementState.Received;

        AchievementReceivedEvent?.Invoke();

        new SimpleAnalyticsEvent("achievement_received", GetAnalyticsData()).Send();
    }

    void SaveState()
    {
        PlayerPrefs.SetInt(StateKey, (int) State);
        PlayerPrefs.Save();
    }
    
    void SaveProgress()
    {
        PlayerPrefs.SetFloat(ProgressKey, Progress);
        PlayerPrefs.Save();
    }

    Dictionary<string, object> GetAnalyticsData()
    {
        return new Dictionary<string, object>()
        {
            {"achievement_name", Name},
            {"balance", Account.Coins}
        };
    }
    
    [Obsolete("Use it only in console commands")]
    public void ResetAchievement()
    {
        State = AchievementState.InProgress;
        Progress = 0;
    }

}


