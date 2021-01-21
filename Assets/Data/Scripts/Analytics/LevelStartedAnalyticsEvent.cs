using System.Linq;
using ScreensScripts;

namespace Puzzle.Analytics
{
    public class LevelStartedAnalyticsEvent: PuzzleAnalyticsEvent
    {
        public LevelStartedAnalyticsEvent()
        {
            LauncherUI.PlayLauncherEvent += PlayLauncherEvent_Handler;
        }
        
        public override string EventName => "level_started";
        
        void PlayLauncherEvent_Handler(PlayLauncherEventArgs args)
        {
            EventData.Clear();
            EventData["level_name"] = args.LevelConfig.Name;
            EventData["difficulty"] = args.LevelConfig.DifficultyLevel;
            EventData["puzzle_name"] = Account.CollectionDefaultItem.Name;
            EventData["puzzle_color"] = Account.CollectionDefaultItem.ActiveColorIndex;
            EventData["freeze_booster"] = Account.GetActiveBoosters().Any(booster => booster is TimeFreezeBooster);
            EventData["live_booster"] = Account.GetActiveBoosters().Any(booster => booster is HeartBooster);
            EventData["balance"] = Account.Coins;
            
            Send();
        }
    }
}