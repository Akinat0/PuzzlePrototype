using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

#if UNITY_EDITOR
public class PuzzleMenu : EditorWindow
{
    
    [MenuItem("PuzzleUtils/Generate Timelines")]
    public static void GenearteTimelines()
    {
        string[] guids = AssetDatabase.FindAssets("Timeline", new []{"Assets/Timelines/RawTimelines"});
    
        foreach (var guid in guids)
        {
            TimelineAsset oldTimeline = (TimelineAsset)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(TimelineAsset));
           
            if (oldTimeline == null)
            {
                Debug.LogError("Asset " + AssetDatabase.GUIDToAssetPath(guid) + " is not timeline");
                continue;
            }

            string processedPath = "Assets/Timelines/ProcessedTimelines/_" + oldTimeline.name + ".playable";
            
            AssetDatabase.CopyAsset(AssetDatabase.GUIDToAssetPath(guid), processedPath);
            
            TimelineAsset newTimeline = (TimelineAsset)AssetDatabase.LoadAssetAtPath(processedPath, typeof(TimelineAsset));

            TimelineProcessor.GenerateNewTimeline(oldTimeline, newTimeline);
        }
    }
}
#endif

