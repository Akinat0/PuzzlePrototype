
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

public class PuzzleAssetsProcessor
{
    public static void ProcessAssets()
    {
        float progress = 0.0f;
        EditorUtility.DisplayProgressBar("Process Assets", "Working...", progress);

        ProcessAtlases();
        ProcessSprites();
        
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
    }


    static void ProcessAtlases()
    {
        string[] guids = AssetDatabase.FindAssets("t:SpriteAtlas");

        for (int i = 0; i < guids.Length; i++)
        {
            string guid = guids[i];
            string path = AssetDatabase.GUIDToAssetPath(guid);
            SpriteAtlas atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);

            bool shouldClose = EditorUtility.DisplayCancelableProgressBar("Process Assets",
                $"{atlas.name}. {i}/{guid.Length}",
                (float)i/guids.Length);
            
            if(shouldClose)
                return;
            
            TextureImporterPlatformSettings settings = atlas.GetPlatformSettings(BuildTarget.iOS.ToString());
            
            settings.overridden = true;
            settings.format = TextureImporterFormat.ETC2_RGBA8;
            atlas.SetPlatformSettings(settings);
            
            AssetDatabase.ImportAsset(path);
        }
    }
    
    static void ProcessSprites()
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture2D");

        for (int i = 0; i < guids.Length; i++)
        {
            string guid = guids[i];
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

            bool shouldClose = EditorUtility.DisplayCancelableProgressBar("Process Textures",
                $"{texture.name}. {i}/{guid.Length}",
                (float)i/guids.Length);
            
            if(shouldClose)
                return;
            
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            
            if(textureImporter == null)
                continue;

            TextureImporterPlatformSettings settings = textureImporter.GetPlatformTextureSettings(BuildTarget.iOS.ToString());
            settings.overridden = true;
            settings.format = TextureImporterFormat.ETC2_RGBA8;
            textureImporter.SetPlatformTextureSettings(settings);
        }
    }
}
