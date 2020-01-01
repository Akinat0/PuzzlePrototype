using System.Collections;
using System.Collections.Generic;
using Abu.Tools;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    
    [SerializeField] private float _partOfScreen = 0.25f;
    [SerializeField] public Transform shape;

    void Start()
    {
        SetScale(_partOfScreen);
    }
    
    void SetScale(float partOfScreen)
    {
        transform.localScale = Vector3.one * 
            ScreenScaler.ScaleToFillPartOfScreen(
                shape.gameObject.GetComponent<SpriteRenderer>(),
                partOfScreen);
    }

    public void ChangeSides()
    {
        shape.Rotate(0, 0, 180);
    }
}
