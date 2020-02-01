using PuzzleScripts;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

#if UNITY_EDITOR
public static class TimelineProcessor
{
     public static void GenerateNewTimeline(TimelineAsset originTimeline, TimelineAsset dstTimeline)
    {
        dstTimeline.DeleteTrack(dstTimeline.markerTrack);
        dstTimeline.CreateMarkerTrack();

        //Clean old timeline
        for (int i=0; i < dstTimeline.markerTrack.GetMarkerCount(); i++)
        {
            dstTimeline.markerTrack.DeleteMarker(dstTimeline.markerTrack.GetMarker(i));
        };
            
        foreach (IMarker output in  originTimeline.markerTrack.GetMarkers())
        {
            EnemyMarker marker = output as EnemyMarker;
            
            if (marker == null)
            {
                Debug.Log("Marker added " + output.time);
                continue;
            }
    
            double deltaTime = EnemyBase.Distance / marker.enemyParams.speed;  
            double creationTime = marker.time - deltaTime;
    
            EnemyNotificationMarker copyMarker = dstTimeline.markerTrack.CreateMarker(typeof(EnemyNotificationMarker), creationTime) as EnemyNotificationMarker;
            
            copyMarker.enemyParams = marker.enemyParams;
            
            Debug.Log("Old time " + marker.time + ", New time " + creationTime);
            
        }
    }
    
}
#endif




