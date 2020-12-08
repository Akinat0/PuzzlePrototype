using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Timeline;

#if UNITY_EDITOR
public class PuzzleMenu : EditorWindow
{
    
    [MenuItem("PuzzleUtils/Generate Timeline")]
    public static void GenerateTimelines()
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
    public static void GenerateBpmMarkers()
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


    [MenuItem("PuzzleUtils/Create Perlin Noise Texture...")]
    public static void CreatePerlinNoiseTexture()
    {
        CreatePerlinNoiseTextureEditorWindow window = EditorWindow.GetWindow<CreatePerlinNoiseTextureEditorWindow>();

        void OnComplete()
        {
            string fileName = window.FileName;
            int width = window.Width;
            int height = window.Height;
            int scale = window.Scale;
            int quality = window.Quality;

            string guid = Selection.assetGUIDs.FirstOrDefault();

            string defaultPath = "Assets/Data/Images/Common";
            
            if (guid != null)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                defaultPath = assetPath.Replace(Path.GetFileName(assetPath), string.Empty);
            }

            string folderPath = EditorUtility.OpenFolderPanel("Choose folder for texture", defaultPath, "");

            byte[] bytes = Utility.CreatePerlinNoiseTexture(width, height, scale).EncodeToJPG(quality);

            string path = Path.Combine(folderPath, $"{fileName}.jpg");

            int count = 0;
            
            while (File.Exists(path))
            {
                path = Path.Combine(folderPath, $"{fileName}_{count}.jpg");
                count++;
            }
            
            File.WriteAllBytes(path, bytes);
            
            AssetDatabase.Refresh();
        }
        
        window.Complete += OnComplete; 
    }

    private class CreatePerlinNoiseTextureEditorWindow : EditorWindow
    {
        public event Action Complete;
        public string FileName => fileName;
        public int Width => width;
        public int Height => height;
        public int Scale => scale;
        public int Quality => quality;

        string fileName = "noise";
        int width = 100;
        int height = 100;
        int scale = 60;
        int quality = 75;

        void OnGUI()
        {
            GUILayout.Label("Create Perlin Noise Texture", EditorStyles.boldLabel);

            fileName = EditorGUILayout.TextField("File Name", fileName);
            width = EditorGUILayout.IntField("Width", width);
            height = EditorGUILayout.IntField("Height", height);
            scale = EditorGUILayout.IntField("Scale", scale);
            quality = EditorGUILayout.IntSlider(quality, 0, 100);

            if (GUILayout.Button("Complete"))
            {
                Complete?.Invoke();
                Close();
            }
        }
    }
}
#endif

