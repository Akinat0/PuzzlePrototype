using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

#if UNITY_EDITOR
namespace Data.Scripts.Editor
{
    public class AutoGenerateTimelines : MonoBehaviour
    {
        private void Start()
        {
            GenearteTimelines();
        }

        private static void GenearteTimelines()
        {
            TimelineAsset[] assets = Selection.GetFiltered<TimelineAsset>(SelectionMode.Assets);
            List<string> guids = new List<string>();
            Debug.Log("AG");

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
    }
}

#endif
