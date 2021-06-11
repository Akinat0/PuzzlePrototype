using System.Linq;

namespace Puzzle.Analytics
{
    public class LevelCompletedAnalyticsEvent : PuzzleAnalyticsEvent
    {
        public LevelCompletedAnalyticsEvent()
        {
            GameSceneManager.LevelCompletedEvent += LevelCompletedEvent_Handler;
        }
        
        public override string EventName => "level_completed";
        
        void LevelCompletedEvent_Handler(LevelCompletedEventArgs args)
        {
            EventData.Clear();
            EventData["level_name"] = args.LevelConfig.Name;
            EventData["difficulty"] = args.LevelConfig.DifficultyLevel;
            EventData["puzzle_name"] = Account.CollectionDefaultItem.Name;
            EventData["puzzle_color"] = Account.CollectionDefaultItem.ActiveColorIndex;
            EventData["lives_left"] = args.LivesLeft;
            EventData["freeze_booster"] = args.BoostersUsed.Any(booster => booster is TimeFreezeBooster);
            EventData["live_booster"] = args.BoostersUsed.Any(booster => booster is HeartBooster);
            EventData["revive"] = args.ReviveUsed;
            EventData["stars"] = Account.Stars.Amount;
            
            Send();
        }
        
    }
}