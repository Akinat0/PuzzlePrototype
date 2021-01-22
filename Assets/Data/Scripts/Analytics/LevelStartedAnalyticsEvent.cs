using System.Linq;

namespace Puzzle.Analytics
{
    public class LevelStartedAnalyticsEvent: PuzzleAnalyticsEvent
    {
        public LevelStartedAnalyticsEvent()
        {
            GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
        }
        
        public override string EventName => "level_started";
        
        void ResetLevelEvent_Handler()
        {
            EventData.Clear();
            
            EventData["level_name"] = GameSceneManager.Instance.LevelConfig.Name;
            EventData["difficulty"] = GameSceneManager.Instance.LevelConfig.DifficultyLevel;
            EventData["puzzle_name"] = Account.CollectionDefaultItem.Name;
            EventData["puzzle_color"] = Account.CollectionDefaultItem.ActiveColorIndex;
            EventData["freeze_booster"] = Account.GetActiveBoosters().Any(booster => booster is TimeFreezeBooster);
            EventData["live_booster"] = Account.GetActiveBoosters().Any(booster => booster is HeartBooster);
            EventData["balance"] = Account.Coins;
            
            Send();
        }
    }
}