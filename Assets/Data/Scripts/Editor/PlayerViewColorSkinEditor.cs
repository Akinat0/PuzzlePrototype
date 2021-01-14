using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerViewColorSkin))]
public class PlayerViewColorSkinEditor : Editor
{
    static GUIStyle richTextLabel;
    public static GUIStyle RichTextLabel
    {
        get
        {
            if (richTextLabel == null)
            {
                richTextLabel = new GUIStyle(EditorStyles.label);
                richTextLabel.richText = true;
            }

            return richTextLabel;
        }
    }
    
    PlayerViewColorSkin Target => target as PlayerViewColorSkin;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(Application.isPlaying)
            return;
        
        DrawColorSkins();
    }

    void DrawColorSkins()
    {
        PlayerViewColorSkin.ColorSkin[] colorSkins = Target.EditorColorSkins;

        for (var i = 0; i < colorSkins.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();

            string colorText = ColorUtility.ToHtmlStringRGB(colorSkins[i].PuzzleColor.Color);

            EditorGUILayout.LabelField($"<color=#{colorText}> {colorSkins[i].PuzzleColor.ID} </color>", RichTextLabel);

            colorSkins[i].Color = EditorGUILayout.ColorField(colorSkins[i].Color);

            EditorGUILayout.EndHorizontal();
        }

        Target.EditorColorSkins = colorSkins;
    }
}
