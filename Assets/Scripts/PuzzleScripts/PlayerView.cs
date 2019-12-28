using System.Collections;
using System.Collections.Generic;
using Abu.Tools;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    
    [SerializeField] private float _partOfScreen = 0.25f;


    void Start()
    {
        SetScale(_partOfScreen);
    }
    
    void SetScale(float partOfScreen)
    {
        transform.localScale = Vector3.one * 
            ScreenScaler.ScaleToFillPartOfScreen(
                gameObject.GetComponent<SpriteRenderer>(),
                partOfScreen);
    }
}
