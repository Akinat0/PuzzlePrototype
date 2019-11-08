using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{

    [SerializeField] private GameObject[] hearts;

    private int _lastHeart; 

    private void Start()
    {
        _lastHeart = hearts.Length - 1;
    }

    public void LoseHeart()
    {
        if (_lastHeart < 0)
        {
            Debug.LogError("LastHeart is " + _lastHeart);
            return;
        }
        hearts[_lastHeart].SetActive(false);
        _lastHeart--;
    } 
}