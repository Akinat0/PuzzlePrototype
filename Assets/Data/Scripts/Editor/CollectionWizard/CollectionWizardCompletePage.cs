using UnityEditor;
using UnityEngine;

public class CollectionWizardCompletePage : WizardPage
{
    public override string Title => "Summary";
    public override string Description => "Check that all things you've done right";


    string Name;
    bool DefaultUnlocked;
    PuzzleColorData[] PuzzleColors;
    CollectionData CollectionData;

    public override void Enter()
    {
        base.Enter();
        Name = Wizard.GetData<string>("collection_wizard_name");
        DefaultUnlocked = Wizard.GetData<bool>("collection_wizard_default_unlocked");
        PuzzleColors = Wizard.GetData<PuzzleColorData[]>("collection_wizard_color_data");
        CollectionData = Wizard.GetData<CollectionData>("collection_wizard_collection_data");
    }


    public override void Draw(Rect rect)
    {
        GUILayout.BeginVertical();

        EditorGUI.TextArea(rect, $"You're about creating puzzle with name(id) {Name}," +
                                 $" it will be unlocked for player by default: {DefaultUnlocked}.\n " +
                                 $"And the puzzle will have {PuzzleColors.Length} different colors.\n " +
                                 $"If all right press \"FINISH\"", Wizard.BlackLabel);
        
        GUILayout.EndVertical();

        if (DrawCompleteButton(rect))
        {
            Execute();
            Wizard.Close();
        }
    }


    void Execute()
    {
        CollectionItem collectionItem = CreateCollectionItem();

        CreateContent(collectionItem);
    }

    CollectionItem CreateCollectionItem()
    {
        CollectionItem collectionItem = ScriptableObject.CreateInstance<CollectionItem>();
        
        collectionItem.Name = Name;
        collectionItem.puzzleColors = PuzzleColors;
        collectionItem.defaultUnlocked = DefaultUnlocked;
        
        AssetDatabase.CreateAsset(collectionItem, $"Assets/Data/Account/Collection/CollectionItems/{Name}CollectionItem.asset");
        CollectionData.AddCollectionItem(collectionItem);
        
        AssetDatabase.Refresh();

        return collectionItem;
    }
    
    void CreateContent(CollectionItem collectionItem)
    {
        const string pathToCollectionFolder = "Assets/CollectionPuzzles";

        string pathToPuzzleFolder = AssetDatabase.CreateFolder(pathToCollectionFolder, collectionItem.Name);
        string pathToAnimations = AssetDatabase.CreateFolder(pathToPuzzleFolder, "Animations");
        string pathToImages = AssetDatabase.CreateFolder(pathToPuzzleFolder, "Images");
        string pathToPrefabs = AssetDatabase.CreateFolder(pathToPuzzleFolder, "Prefabs");
    }

    void CreateAnimations(string animationsFolder)
    {
        
    }

    void CreatePrefabs(string prefabsFolder)
    {
        
    }
}
