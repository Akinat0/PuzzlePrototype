using System.Collections;
using System.Collections.Generic;
using Abu.Tools;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundView : MonoBehaviour
{
    void Start()
    {
        transform.localScale = 
            Vector3.one * ScreenScaler.BestFit(GetComponent<SpriteRenderer>());
    }

}
