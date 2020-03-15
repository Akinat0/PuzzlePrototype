using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PuzzleCollectionItem", menuName = "Puzzle/CreateCollectionItem", order = 51)]
public class CollectionItem : ScriptableObject
{
    [SerializeField] private GameObject m_Item;

    public GameObject Item => m_Item;
}
