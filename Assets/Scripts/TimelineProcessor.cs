using PuzzleScripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public static class TimelineProcessor
{
    public static void GenerateNewTimeline(TimelineAsset t)
    {
        foreach (IMarker output in  t.markerTrack.GetMarkers())
        {
            AssetDatabase.SaveAssets();
            EnemyMarker marker = output as EnemyMarker;
            
            if (marker == null)
            {
                Debug.Log("fuck");
                continue;
            }

            double deltaTime = EnemyBase.Distance * marker.enemyParams.speed;  
            double creationTime = marker.time - deltaTime;
            
            Debug.Log("Old Time " + marker.time + " new Time " + creationTime);

            marker.time = creationTime;
    
        }
    }
    public static void GenerateNewTimeline(TimelineAsset originTimeline, TimelineAsset dstTimeline)
    {
        dstTimeline.CreateMarkerTrack();
        
        foreach (IMarker output in  originTimeline.markerTrack.GetMarkers())
        {
            EnemyMarker marker = output as EnemyMarker;
            
            if (marker == null)
            {
                Debug.Log("Marker added " + output.time);
                continue;
            }
    
            double deltaTime = EnemyBase.Distance / marker.enemyParams.speed;  
            double creationTime = marker.time - deltaTime + 0.1f;
    
            EnemyNotificationMarker copyMarker = dstTimeline.markerTrack.CreateMarker(typeof(EnemyNotificationMarker), creationTime) as EnemyNotificationMarker;
            copyMarker.enemyParams = marker.enemyParams;
            
            Debug.Log("Old time " + marker.time + ", New time " + creationTime);
            
        }
    }
    
}


public class EnemyMarker : Marker
{
    [SerializeField] public EnemyParams enemyParams;
}
//Kind of crutch
[CustomStyle("TimelineAction")]
public class EnemyNotificationMarker : Marker, INotification
{
    [SerializeField] public EnemyParams enemyParams;

    public PropertyName id { get; }
}

