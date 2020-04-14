using System.Collections;
using System.Collections.Generic;
using Puzzle;
using UnityEngine;

public class CallFX : MonoBehaviour
{
    void Start()
    {
        VFXManager.Instance.CallCrosslightEffect(transform.position);
    }

}
