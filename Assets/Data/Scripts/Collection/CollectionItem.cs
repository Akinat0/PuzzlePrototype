using System;
using System.Collections.Generic;
using System.Linq;
using Puzzle;
using Puzzle.Analytics;
using UnityEngine;

[CreateAssetMenu(fileName = "New PuzzleCollectionItem", menuName = "Puzzle/CreateCollectionItem", order = 51)]
public class CollectionItem : ScriptableObject
{
    [SerializeField] public string Name;
    [SerializeField] public PuzzleVariant[] puzzleVariants;
    [SerializeField] public bool defaultUnlocked;
    [SerializeField] public PuzzleColorData[] puzzleColors;

    public event Action<bool> OnUnlockedEvent;
    public event Action<int> OnActiveColorChangedEvent;
    
    public bool DefaultUnlocked => defaultUnlocked;

    bool unlocked;
    public bool Unlocked
    {
        get => unlocked;
        set
        {
            if (unlocked == value)
                return;

            unlocked = value;
            OnUnlockedEvent?.Invoke(unlocked);
        }
    }

    public PuzzleColorData[] PuzzleColors => puzzleColors;
    public PuzzleColorData? ActiveColorData => PuzzleColors.Length > ActiveColorIndex ? PuzzleColors[ActiveColorIndex] : (PuzzleColorData?) null;

    int activeColorIndex;
    public int ActiveColorIndex
    {
        get => activeColorIndex;
        set
        {
            if(activeColorIndex == value)
                return;
            
            int prevColorIndex = activeColorIndex; 
            activeColorIndex = value;
            
            OnActiveColorChangedEvent?.Invoke(activeColorIndex);
            SendPuzzleColorChangedAnalyticsEvent(prevColorIndex, activeColorIndex);
        }
    }

    public int ID => Name.GetHashCode();
    
    public GameObject GetPuzzleVariant(PuzzleSides sides)
    {
        return puzzleVariants.FirstOrDefault(variant => variant.Sides == sides).Puzzle;
    }

    public GameObject GetAnyPuzzleVariant()
    {
        return puzzleVariants.FirstOrDefault().Puzzle;
    }

    void SendPuzzleColorChangedAnalyticsEvent(int prevColorIndex, int newColorIndex)
    {
        Dictionary<string, object> eventData = new Dictionary<string, object>()
        {
            {"puzzle_name", Name},
            {"previous_color", prevColorIndex},
            {"puzzle_color", newColorIndex},
            {"stars", Account.Stars.Amount}
        };
        
        new SimpleAnalyticsEvent("puzzle_color_changed", eventData).Send();
    }
    
    #if UNITY_EDITOR

    public static void SetEditorPuzzleVariants(CollectionItem item, PuzzleVariant[] puzzleVariants)
    {
        item.puzzleVariants = puzzleVariants;
    }

    #endif
    
}

[Serializable]
public struct PuzzleVariant
{
    public PuzzleVariant(PuzzleSides sides, GameObject prefab)
    {
        this.sides = sides;
        this.puzzle = prefab;
    }
    
    [SerializeField] private PuzzleSides sides;
    [SerializeField] private GameObject puzzle;

    public PuzzleSides Sides => sides;
    public GameObject Puzzle => puzzle;
}

[Serializable]
public struct PuzzleColorData
{
    public PuzzleColorData(string id, Color color, bool defaultUnlocked)
    {
        this.id = id;
        this.color = color;
        this.defaultUnlocked = defaultUnlocked;
        IsUnlocked = false;
    }

    [SerializeField] string id;
    [SerializeField] Color color;
    [SerializeField] bool defaultUnlocked;

    public bool IsUnlocked { set; get; }
    
    public string ID => id;
    public Color Color => color;
    public bool DefaultUnlocked => defaultUnlocked;

    public bool Equals(PuzzleColorData other)
    {
        return id == other.id && color.Equals(other.color);
    }
}
