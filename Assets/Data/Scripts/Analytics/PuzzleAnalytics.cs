using System.Collections.Generic;
using Puzzle.Analytics;
using UnityEngine;
using UnityEngine.Analytics;

public class PuzzleAnalytics
{
    readonly Dictionary<string, PuzzleAnalyticsEvent> Events = new Dictionary<string, PuzzleAnalyticsEvent>();
    public PuzzleAnalytics()
    {
        Analytics.enabled = true;
        AnalyticsEvent.debugMode = true;
        Debug.Log(GetSessionInfo());

        RegisterEvent(new LevelCompletedAnalyticsEvent());
        RegisterEvent(new LevelClosedAnalyticsEvent());
        RegisterEvent(new LevelStartedAnalyticsEvent());
        RegisterEvent(new PlayerDiedAnalyticsEvent());
        RegisterEvent(new PlayerRevivedAnalyticsEvent());
        
        LoadAnalytics();
    }
    
    public bool IsEnabled { get; private set; }
    
    public void RegisterEvent(PuzzleAnalyticsEvent analyticsEvent)
    {
        if(Events.ContainsKey(analyticsEvent.EventName))
            Debug.LogWarning($"Event with name {analyticsEvent.EventName} registered several times");

        Events[analyticsEvent.EventName] = analyticsEvent;
    }
    
    public string GetSessionInfo()
    {
        return $"[Analytics] SessionInfo. userID: {AnalyticsSessionInfo.userId} ; sessionState: {AnalyticsSessionInfo.sessionState} ; sessionID: {AnalyticsSessionInfo.sessionId} ; sessionElapsedTime: {AnalyticsSessionInfo.sessionElapsedTime}";
    }

    public void Enable()
    {
        if(IsEnabled)
            return;

        IsEnabled = true;
        SaveAnalytics();
    }

    public void Disable()
    {
        if(!IsEnabled)
            return;
        IsEnabled = false;
        SaveAnalytics();
    }

    void SaveAnalytics()
    {
        PlayerPrefs.SetInt(AnalyticsEnabledKey, IsEnabled ? 1 : 0);
    }

    void LoadAnalytics()
    {
        IsEnabled = PlayerPrefs.GetInt(AnalyticsEnabledKey, 1) == 1;
    }
    string AnalyticsEnabledKey => "analytics_enabled_key";
}
