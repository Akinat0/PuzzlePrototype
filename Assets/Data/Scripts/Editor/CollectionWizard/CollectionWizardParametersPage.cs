using System.Linq;
using System.Text;
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
    Rarity rarity = Rarity.Common;

    readonly List<PuzzleColorData> PuzzleColors = new List<PuzzleColorData>();

    readonly string[] rarityValues = { "Common", "Rare", "Epic" };

    protected override bool Valid => !string.IsNullOrEmpty(Name.Trim()) && 
         PuzzleColors.Count <= 5 &&
         !PuzzleColors.Any(puzzleColor => string.IsNullOrEmpty(puzzleColor.ID.Trim())) &&
         PuzzleColors.Any(puzzleColor => puzzleColor.DefaultUnlocked);

    protected override string GetErrors()
    {
        StringBuilder errors = new StringBuilder();

        if (string.IsNullOrEmpty(Name.Trim()))
            errors.AppendLine("Puzzle Name shouldn't be empty");
        if(PuzzleColors.Any(puzzleColor => string.IsNullOrEmpty(puzzleColor.ID.Trim())))
            errors.AppendLine("Puzzle Color ID shouldn't be empty");
        if (!PuzzleColors.Any(puzzleColor => puzzleColor.DefaultUnlocked))
            errors.AppendLine("At least one color must be unlocked by default");
        if(PuzzleColors.Count > 5)
            errors.AppendLine("Max puzzle colors size is 5");

        return errors.ToString();
    }

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

        puzzleColorsReordableList = new ReorderableList(PuzzleColors, typeof(PuzzleColorData));

        puzzleColorsReordableList.drawHeaderCallback = DrawColorsTitle;
        puzzleColorsReordableList.drawElementCallback = DrawColorsElement;
        puzzleColorsReordableList.onAddCallback = OnAddColor;
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
        
        //int hack to receive correct enum value
        EditorGUILayout.LabelField("Rarity", Wizard.BlackLabel);
        rarity = (Rarity) (EditorGUILayout.Popup((int)rarity - 1, rarityValues) + 1);

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

    void OnAddColor(ReorderableList reorderableList)
    {
        int listCount = reorderableList.count;
        reorderableList.list.Add(new PuzzleColorData($"Color_{listCount}", Color.white, false));
    }
}
