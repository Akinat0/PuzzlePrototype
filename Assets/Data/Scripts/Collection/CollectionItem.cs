using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Puzzle;
using UnityEngine;

[CreateAssetMenu(fileName = "New PuzzleCollectionItem", menuName = "Puzzle/CreateCollectionItem", order = 51)]
public class CollectionItem : ScriptableObject
{
    [SerializeField] public string Name; 
    [SerializeField] private PuzzleVariant[] puzzleVariants;


    public GameObject GetPuzzleVariant(PuzzleSides sides)
    {
        return puzzleVariants.FirstOrDefault(variant => variant.Sides == sides).Puzzle;
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
