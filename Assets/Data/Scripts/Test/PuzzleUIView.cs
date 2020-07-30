using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleUIView : MonoBehaviour
{
    [SerializeField] SetPuzzlesForCamera puzzlesForCamera;
    [SerializeField] CollectionItem collectionItem;
    void Start()
    {
        GetComponent<RawImage>().uvRect = puzzlesForCamera.GetPuzzleUV(collectionItem.Name); 
    }

}
