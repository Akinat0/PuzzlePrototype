using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(LevelsData))]
public class LevelsDataEditor : Editor
{
    ReorderableList levelsList;

    void OnEnable()
    {
        levelsList = new ReorderableList(serializedObject, serializedObject.FindProperty("_LevelItems"), 
            true, true, true, true);
        levelsList.drawElementCallback = DrawElement;
        levelsList.drawHeaderCallback = DrawHeader;
        levelsList.onAddCallback = OnAddCallback;
        levelsList.onRemoveCallback = OnRemoveCallback;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        levelsList.DoLayoutList();
    }

    void DrawElement(Rect rect, int index, bool active, bool focused)
    {
        EditorGUI.PropertyField(rect, serializedObject.FindProperty("_LevelItems").GetArrayElementAtIndex(index));
    }
    
    void DrawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Levels");
    }

    void OnAddCallback(ReorderableList list)
    {
        serializedObject.FindProperty("_LevelItems").arraySize++;
        serializedObject.ApplyModifiedProperties();
    }
    
    void OnRemoveCallback(ReorderableList list)
    {
        serializedObject.FindProperty("_LevelItems").arraySize--;
        serializedObject.ApplyModifiedProperties();
    }
}
