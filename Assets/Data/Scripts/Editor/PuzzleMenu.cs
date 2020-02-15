using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
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

            TimelineAsset newTimeline = (TimelineAsset)AssetDatabase.LoadAssetAtPath(processedPath, typeof(TimelineAsset));

            if (newTimeline == null) {

                AssetDatabase.CopyAsset(AssetDatabase.GUIDToAssetPath(guid), processedPath);
                newTimeline = (TimelineAsset)AssetDatabase.LoadAssetAtPath(processedPath, typeof(TimelineAsset));
            }
            
            TimelineProcessor.GenerateNewTimeline(oldTimeline, newTimeline);
        } 
    }

    [MenuItem("PuzzleUtils/Generate Bpm Markers")]
    public static void GenearteBpmMarkers()
    {
        string[] folders = AssetDatabase.GetSubFolders("Assets/Timelines/RawTimelines/bpm");
        foreach(string folder in folders)
        {
            string folderName;
            folderName = Path.GetFileName(folder);
            int bpm = Int32.Parse(folderName);
            Debug.LogError("bpm = " + bpm);
            string[] guids = AssetDatabase.FindAssets("Timeline", new[] { "Assets/Timelines/RawTimelines/bpm/" + folderName });
            foreach(var guid in guids)
            {
                TimelineAsset oldTimeline = (TimelineAsset)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(TimelineAsset));
                if (oldTimeline == null)
                {
                    Debug.LogError("Asset " + AssetDatabase.GUIDToAssetPath(guid) + " is not timeline");
                    continue;
                }
                Debug.LogError("Find old timeline");
                string processedPath = "Assets/Timelines/RawTimelines/bpm/" + folderName + "/bpm_" + oldTimeline.name + ".playable";

                TimelineAsset newTimeline = (TimelineAsset)AssetDatabase.LoadAssetAtPath(processedPath, typeof(TimelineAsset));

                if (newTimeline == null)
                {

                    AssetDatabase.CopyAsset(AssetDatabase.GUIDToAssetPath(guid), processedPath);
                    newTimeline = (TimelineAsset)AssetDatabase.LoadAssetAtPath(processedPath, typeof(TimelineAsset));
                }

                Debug.LogError("Create new timeline");
                TimelineProcessor.GenerateBpmTimeline(oldTimeline, newTimeline, bpm);
            }
        }
    }
}
#endif

