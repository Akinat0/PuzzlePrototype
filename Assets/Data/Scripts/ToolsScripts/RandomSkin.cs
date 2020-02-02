using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomSkin : MonoBehaviour
{
    [SerializeField] Sprite[] Skins; 

    void Start()
    {
        int skinNumber = Random.Range(0, Skins.Length);
        GetComponent<SpriteRenderer>().sprite = Skins[skinNumber];
    }

}
