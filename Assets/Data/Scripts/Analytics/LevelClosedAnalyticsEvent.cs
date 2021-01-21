using ScreensScripts;

namespace Puzzle.Analytics
{
    public class LevelClosedAnalyticsEvent : PuzzleAnalyticsEvent
    {
        public override string EventName => "level_closed";

        public LevelClosedAnalyticsEvent()
        {
            LauncherUI.GameEnvironmentUnloadedEvent += GameEnvironmentUnloadedEvent_Handler;
        }
        
        void GameEnvironmentUnloadedEvent_Handler(GameSceneUnloadedArgs args)
        {
            if(args.Reason != GameSceneUnloadedArgs.GameSceneUnloadedReason.LevelClosed)
                return;

            EventData.Clear();
            
            EventData["level_name"] = args.LevelConfig.Name;
            EventData["difficulty"] = args.LevelConfig.DifficultyLevel;
            
            Send();
        }
    }
}