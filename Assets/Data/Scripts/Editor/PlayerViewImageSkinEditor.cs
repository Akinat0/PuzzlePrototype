using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerViewImageSkin))]
public class PlayerViewImageSkinEditor : Editor
{
    PlayerViewImageSkin Target => target as PlayerViewImageSkin;
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(Application.isPlaying)
            return;
        
        DrawImageSkins();
    }
    
    void DrawImageSkins()
    {
        PlayerViewImageSkin.ImageSkin[] imageSkins = Target.EditorImageSkins;

        for (var i = 0; i < imageSkins.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();

            string colorText = ColorUtility.ToHtmlStringRGB(imageSkins[i].PuzzleColor.Color);

            EditorGUILayout.LabelField($"<color=#{colorText}> {imageSkins[i].PuzzleColor.ID} </color>", PlayerViewColorSkinEditor.RichTextLabel);

            imageSkins[i].Image = (Sprite)EditorGUILayout.ObjectField(imageSkins[i].Image, typeof(Sprite), false);

            EditorGUILayout.EndHorizontal();
        }

        Target.EditorImageSkins = imageSkins;
    }
}