
using System.Collections.Generic;
using Abu.Tools;
using UnityEditor;
using UnityEngine;

public class CollectionWizardFirstPage : WizardPage
{

    public override string Title => "Collection";
    public override string Description => "Puzzles Collection";
    
    CollectionData CollectionData;
    Vector2 puzzlesScrollPos;
    
    readonly Dictionary<int, Texture2D> puzzleTextures = new Dictionary<int, Texture2D>();
    
    public override void Enter()
    {
        base.Enter();

        CollectionData = GetCollectionData();
    }

    public override void Exit()
    {
        base.Exit();
        
        Wizard.SetData("collection_wizard_collection_data", CollectionData);
    }

    public override void Draw(Rect rect)
    {
        GUILayout.BeginArea(rect);
        
        DrawPuzzles(rect);
        
        GUILayout.EndArea();
    }

    CollectionData GetCollectionData()
    {
        return AssetDatabase.LoadAssetAtPath<CollectionData>("Assets/Data/Account/CollectionData.asset");
    }

    void DrawPuzzles(Rect rect)
    {
        int itemsInRow = (int) rect.width / 100 - 1;
        bool horizontalLayout = false;
        
        GUILayout.BeginVertical();
        puzzlesScrollPos = EditorGUILayout.BeginScrollView(puzzlesScrollPos);
        
        int length = CollectionData.CollectionItems.Length + 1;
        
        for (int i = 0; i < length; i++)
        {
            if (!horizontalLayout)
            {
                GUILayout.BeginHorizontal();
                horizontalLayout = true;
            }

            if (i != length - 1)
                DrawCollectionPuzzleButton(CollectionData.CollectionItems[i]);
            else
                DrawAddNewButton();
            
            if ((i + 1) % itemsInRow == 0)
            {
                GUILayout.EndHorizontal();
                horizontalLayout = false;
            }
        }
        
        EditorGUILayout.EndScrollView();
        
        GUILayout.EndVertical();
    }


    void DrawCollectionPuzzleButton(CollectionItem collectionItem)
    {
        GUILayout.Button(new GUIContent(GetPuzzleTexture(collectionItem), collectionItem.Name),
            GUILayout.Width(100), GUILayout.Height(100));
    }
    
    void DrawAddNewButton()
    {
        if(GUILayout.Button(new GUIContent(EditorGUIUtility.FindTexture("CreateAddNew@2x"), "Add new"), GUILayout.Width(100), GUILayout.Height(100)))
            Complete();
    }

    Texture2D GetPuzzleTexture(CollectionItem collectionItem)
    {
        if (puzzleTextures.ContainsKey(collectionItem.ID))
            return puzzleTextures[collectionItem.ID];
        
        PlayerView playerView = PlayerView.Create(null, collectionItem);
        
        Texture2D texture = CaptureUtility.Capture(playerView.shape.GetComponent<SpriteRenderer>(), inOtherLayer:false);
        
        Object.DestroyImmediate(playerView.gameObject);
        
        puzzleTextures[collectionItem.ID] = texture;

        return texture;
    }
}
