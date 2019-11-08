using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoHelper : MonoBehaviour
{
    void OnDisable()
    {
        StopAllCoroutines();
    }
}
