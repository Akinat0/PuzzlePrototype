using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(PlayerViewColorSkin), true)]
public class PlayerViewColorSkinEditor : Editor
{
    CollectionItem CollectionItem;
    PlayerViewColorSkin Target;
    void OnEnable()
    {
        if(Application.isPlaying)
            return;
        Target = target as PlayerViewColorSkin;
        CollectionItem = GetCollectionItem();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(Application.isPlaying)
            return;
        
        DrawMap();
    }

    void DrawMap()
    {

        if (CollectionItem == null)
        {
            EditorGUILayout.HelpBox("Collection item can't be found", MessageType.Error);
            return;
        }
        
        for (int i = 0; i < CollectionItem.PuzzleColors.Length; i++)
        {
            PuzzleColorData colorData = CollectionItem.PuzzleColors[i];
            
            if(!Target.ColorIDs.Contains(colorData.ID))
            {
                Target.ColorIDs.Add(colorData.ID);
                Target.Colors.Add(Color.white);
            }
            
            if(colorData.Equals(default))
            {
                EditorGUILayout.HelpBox($"ColorData {colorData.ID} can't be found", MessageType.Error);
                return;
            }
            
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.ColorField(colorData.Color);

            if (Target.Colors.Count <= i)
            {
                EditorGUILayout.HelpBox($"Color {i} doesn't exists", MessageType.Error);
                EditorGUILayout.EndHorizontal();
                return;
            }

            EditorGUI.BeginChangeCheck();
            
            Target.Colors[i] = EditorGUILayout.ColorField(Target.Colors[i]);
            
            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(target);
            
            EditorGUILayout.EndHorizontal();
        }
    }
    
    CollectionItem GetCollectionItem()
    {
        PrefabStage prefabStage = PrefabStageUtility.GetPrefabStage(Target.gameObject);
        string pathToPrefab = prefabStage.prefabAssetPath;
        
        string[] separatedPath = pathToPrefab.Split('/');
        string directoryName = separatedPath[separatedPath.Length - 2];
        
        foreach (string itemGuid in AssetDatabase.FindAssets("CollectionItem", new[] {"Assets/Data/Account/Collection/CollectionItems"}))
        {
            CollectionItem item = AssetDatabase.LoadAssetAtPath<CollectionItem>(AssetDatabase.GUIDToAssetPath(itemGuid));
            if (item.Name == directoryName)
                return item;
        }

        return null;
    }
}
