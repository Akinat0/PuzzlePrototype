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
            IMarker marker = dstTimeline.markerTrack.GetMarker(i);
            if(marker is BpmMarker || marker is EnemyNotificationMarker || marker is EnemyMarker)
                dstTimeline.markerTrack.DeleteMarker(marker);
        };
            
        foreach (IMarker output in  originTimeline.markerTrack.GetMarkers())
        {
            switch (output)
            {
                case EnemyMarker enemyMarker:
                {
                    if (enemyMarker == null)
                    {
                        Debug.Log("Marker added " + output.time);
                        continue;
                    }
    
                    double deltaTime = EnemyBase.Distance / enemyMarker.enemyParams.speed;  
                    double creationTime = enemyMarker.time - deltaTime;
    
                    EnemyNotificationMarker copyMarker = dstTimeline.markerTrack.CreateMarker(typeof(EnemyNotificationMarker), creationTime) as EnemyNotificationMarker;
            
                    copyMarker.enemyParams = enemyMarker.enemyParams;
            
                    Debug.Log("Old time " + enemyMarker.time + ", New time " + creationTime);
                    break;
                }

                case NoteMarker noteMarker:
                {
                    dstTimeline.markerTrack.CreateMarker(typeof(NoteMarker), noteMarker.time);
                    Debug.Log("Note marker created " + noteMarker.time);
                    break;
                }
            }
        }
    }
    
    
    
    public static void GenerateBpmTimeline(TimelineAsset dstTimeline, int bpm)
    {
        
        float currentTime = 0f;
        float stepTime = 60.0f / bpm;
        
        while (currentTime < dstTimeline.duration)
        {
            dstTimeline.markerTrack.CreateMarker(typeof(BpmMarker), currentTime);
            currentTime += stepTime;
        }
    }

}
#endif




