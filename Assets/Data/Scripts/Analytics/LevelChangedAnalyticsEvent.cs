using ScreensScripts;

namespace Puzzle.Analytics
{
    public class LevelChangedAnalyticsEvent : PuzzleAnalyticsEvent
    {
        public LevelChangedAnalyticsEvent()
        {
            LauncherUI.LevelChangedEvent += LevelChangedEvent_Handler;
        }

        public override string EventName => "level_changed";
        
        void LevelChangedEvent_Handler(LevelChangedEventArgs args)
        {
            //Prevent event duplication
            if(EventData.ContainsKey("level_name") && (string) EventData["level_name"] == args.LevelConfig.Name)
                return;
                
            EventData.Clear();
            
            EventData["level_name"] = args.LevelConfig.Name;
            EventData["balance"] = Account.Coins;
            Send();
        }
    }
}