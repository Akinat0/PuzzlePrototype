using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(TutorialFadeImage))]
public class TutorialFadeImageEditor : ImageEditor
{
    SerializedProperty holeSizeProperty;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        holeSizeProperty = serializedObject.FindProperty("holeSize");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        SpriteGUI();
        AppearanceControlsGUI();
        EditorGUILayout.PropertyField(holeSizeProperty);

        serializedObject.ApplyModifiedProperties();
    }
}
