using System;
using System.Linq;
using Puzzle;
using UnityEngine;

[CreateAssetMenu(fileName = "New PuzzleCollectionItem", menuName = "Puzzle/CreateCollectionItem", order = 51)]
public class CollectionItem : ScriptableObject
{
    [SerializeField] public string Name;
    [SerializeField] private PuzzleVariant[] puzzleVariants;
    [SerializeField] private bool defaultUnlocked;

    public event Action<bool> OnUnlockedEvent;
    
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
    
    public int ID => Name.GetHashCode();
    
    public GameObject GetPuzzleVariant(PuzzleSides sides)
    {
        return puzzleVariants.FirstOrDefault(variant => variant.Sides == sides).Puzzle;
    }

    public GameObject GetAnyPuzzleVariant()
    {
        return puzzleVariants.FirstOrDefault().Puzzle;
    }
}

[Serializable]
public struct PuzzleVariant
{
    [SerializeField] private PuzzleSides sides;
    [SerializeField] private GameObject puzzle;

    public PuzzleSides Sides => sides;
    public GameObject Puzzle => puzzle;
}
