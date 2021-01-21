using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace Puzzle.Analytics
{
    public abstract class PuzzleAnalyticsEvent
    {
        public abstract string EventName { get; }
        
        protected readonly Dictionary<string, object> EventData = new Dictionary<string, object>();

        public void Send()
        {
            AnalyticsResult result;

            if(EventData.Count > 0)
                result = AnalyticsEvent.Custom(EventName, EventData);
            else
                result = AnalyticsEvent.Custom(EventName);
            
            Debug.Log($"<color=blue>[Analytics] Event {EventName} sent. Result is {result}</color>");
        }
    }
}