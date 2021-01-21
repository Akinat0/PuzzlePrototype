using System.Collections.Generic;

namespace Puzzle.Analytics
{
    public class SimpleAnalyticsEvent : PuzzleAnalyticsEvent
    {
        public SimpleAnalyticsEvent(string eventName, (string, object) eventData) : this(eventName,
            new Dictionary<string, object> {{eventData.Item1, eventData.Item2}})
        { }
        
        public SimpleAnalyticsEvent(string eventName, IDictionary<string, object> eventData = null)
        {
            EventName = eventName;
            
            if(eventData == null)
                return;
            
            foreach (KeyValuePair<string, object> kvp in eventData)
                EventData[kvp.Key] = kvp.Value;
        }
        
        public override string EventName { get; }
    }
}