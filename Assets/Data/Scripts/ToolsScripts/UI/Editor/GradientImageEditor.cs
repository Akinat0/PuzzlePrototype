using Abu.Tools.UI;
using UnityEditor;
using UnityEditor.UI;


[CustomEditor(typeof(GradientImage))]
public class GradientImageEditor : ImageEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        EditorGUI.BeginChangeCheck();
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("firstColor"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("secondColor"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("inverseGradient"));
    
        if(EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }
}