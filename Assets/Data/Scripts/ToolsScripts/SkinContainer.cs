using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinContainer : MonoBehaviour
{
    public Sprite[] sprites;

    private int index = 0;
    
    public void IncrementPhase()
    {
        index++;
        if(index < sprites.Length)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[index];
        }
    }
}
