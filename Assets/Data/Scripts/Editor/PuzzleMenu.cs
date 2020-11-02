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
    
    [MenuItem("PuzzleUtils/Generate Timeline")]
    public static void GenearteTimelines()
    {
        TimelineAsset[] assets = Selection.GetFiltered<TimelineAsset>(SelectionMode.Assets);
        List<string> guids = new List<string>();
        
        foreach (TimelineAsset asset in assets)
            guids.Add(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(asset)));
        
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

        if (folders.Length == 0)
        {
            Debug.LogError("There's no bpm folders");
            return;
        }

        foreach(string folder in folders)
        {
            string folderName;
            folderName = Path.GetFileName(folder);
            int bpm = Int32.Parse(folderName);
            string[] guids = AssetDatabase.FindAssets("Timeline", new[] { "Assets/Timelines/RawTimelines/bpm/" + folderName });
            foreach(var guid in guids)
            {
                TimelineAsset oldTimeline = (TimelineAsset)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(TimelineAsset));
                if (oldTimeline == null)
                {
                    Debug.LogError("Asset " + AssetDatabase.GUIDToAssetPath(guid) + " is not timeline");
                    continue;
                }
                string processedPath = "Assets/Timelines/RawTimelines/bpm/" + folderName + "/bpm_" + oldTimeline.name + ".playable";

                TimelineAsset newTimeline = (TimelineAsset)AssetDatabase.LoadAssetAtPath(processedPath, typeof(TimelineAsset));

                if (newTimeline == null)
                {

                    AssetDatabase.CopyAsset(AssetDatabase.GUIDToAssetPath(guid), processedPath);
                    newTimeline = (TimelineAsset)AssetDatabase.LoadAssetAtPath(processedPath, typeof(TimelineAsset));
                }


                TimelineProcessor.GenerateBpmTimeline(newTimeline, bpm);
            }
        }
    }
    
    [MenuItem("PuzzleUtils/Delete local")]
    public static void DeleteLocal()
    {
        PlayerPrefs.DeleteAll();
    }
}
#endif

