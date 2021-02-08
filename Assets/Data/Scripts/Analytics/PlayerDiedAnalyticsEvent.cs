using System.Linq;

namespace Puzzle.Analytics
{
    public class PlayerDiedAnalyticsEvent : PuzzleAnalyticsEvent
    {
        public override string EventName => "player_died";

        public PlayerDiedAnalyticsEvent()
        {
            GameSceneManager.PlayerDiedEvent += PlayerDiedEvent_Handler;
        }
        
        void PlayerDiedEvent_Handler()
        {
            EventData.Clear();
            
            EventData["level_name"] = GameSceneManager.Instance.LevelConfig.Name;
            EventData["difficulty"] = GameSceneManager.Instance.LevelConfig.DifficultyLevel;
            EventData["level_time"] = GameSceneManager.Instance.Session.CurrentLevelTime;
            EventData["freeze_booster"] = GameSceneManager.Instance.Session.ActiveBoosters.Any(booster => booster is TimeFreezeBooster);
            EventData["live_booster"] = GameSceneManager.Instance.Session.ActiveBoosters.Any(booster => booster is HeartBooster);
            EventData["revive"] = GameSceneManager.Instance.Session.ReviveUsed;
            
            Send();
        }
    }
}