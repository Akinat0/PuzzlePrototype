using Boo.Lang;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

public class CollectionWizardParametersPage : WizardPage
{
    public override string Title => "Parameters";
    public override string Description => "Here you should assign a name and other parameters";


    ReorderableList puzzleColorsReordableList;
    
    string Name = string.Empty;
    bool DefaultUnlocked;
    List<PuzzleColorData> PuzzleColors;
    
    public override void Draw(Rect rect)
    {
        GUILayout.BeginArea(rect);
        
        DrawParameters(rect);
        
        GUILayout.EndArea();
        
        DrawNavigationButtons(rect);
    }

    public override void Enter()
    {
        base.Enter();

        PuzzleColors = new List<PuzzleColorData>();
        
        puzzleColorsReordableList = new ReorderableList(PuzzleColors, typeof(PuzzleColorData));

        puzzleColorsReordableList.drawHeaderCallback = DrawColorsTitle;
        puzzleColorsReordableList.drawElementCallback = DrawColorsElement;
    }

    public override void Exit()
    {
        base.Exit();
        
        Wizard.SetData("collection_wizard_name", Name);
        Wizard.SetData("collection_wizard_default_unlocked", DefaultUnlocked);
        Wizard.SetData("collection_wizard_color_data", PuzzleColors.ToArray());
    }

    void DrawParameters(Rect rect)
    {
        EditorGUILayout.BeginVertical();
        
        EditorGUILayout.LabelField("Puzzle Name", Wizard.BlackLabel);
        Name = EditorGUILayout.TextField(Name);
        
        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("Default Unlocked", Wizard.BlackLabel);
        DefaultUnlocked = EditorGUILayout.Toggle(DefaultUnlocked);
        
        EditorGUILayout.Space(10);
        
        puzzleColorsReordableList.DoLayoutList();
        
        EditorGUILayout.EndVertical();
    }

    void DrawColorsTitle(Rect rect)
    {
        EditorGUI.LabelField(rect, "Colors");      
    }

    void DrawColorsElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        PuzzleColorData puzzleColorData = PuzzleColors[index];

        Rect idRect = new Rect(rect.position, new Vector2(rect.width / 2 - 9, rect.height));
        Rect colorRect = new Rect(rect.position + new Vector2(rect.width / 2 - 7, 0), new Vector2(rect.width / 2 - 7, rect.height));
        Rect defaultUnlockedRect = new Rect(rect.position + new Vector2(rect.width - 10, 0), new Vector2(10, rect.height));
        
        string id = EditorGUI.TextField(idRect, puzzleColorData.ID);
        Color color = EditorGUI.ColorField(colorRect, puzzleColorData.Color);
        bool defaultUnlocked = EditorGUI.Toggle(defaultUnlockedRect, puzzleColorData.DefaultUnlocked);
        
        PuzzleColors[index] = new PuzzleColorData(id, color, defaultUnlocked);
    }

}
