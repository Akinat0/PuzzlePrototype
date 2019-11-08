using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataContainer : MonoBehaviour
{
    private Dictionary<string, int> _integerData;
    void Start()
    {
        DontDestroyOnLoad(this);
    }
}
