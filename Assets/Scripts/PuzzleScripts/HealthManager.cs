using System;
using System.Collections;
using System.Collections.Generic;
using Puzzle;
using UnityEngine;

public class HealthManager : MonoBehaviour
{

    [SerializeField] private GameObject[] hearts;

    private int _lastHeart; 

    private void Start()
    {
        _lastHeart = hearts.Length - 1;
    }

    void LoseHeart()
    {
        if (_lastHeart < 0)
        {
            Debug.LogError("LastHeart is " + _lastHeart);
            return;
        }
        hearts[_lastHeart].SetActive(false);
        _lastHeart--;
    }


    private void OnEnable()
    {
        GameSceneManager.ResetLevelEvent += ResetLevelEvent_Handler;
        GameSceneManager.PlayerLosedHpEvent += PlayerLosedHpEvent_Handler;
    }

    private void OnDisable()
    {
        GameSceneManager.ResetLevelEvent -= ResetLevelEvent_Handler;
        GameSceneManager.PlayerLosedHpEvent -= PlayerLosedHpEvent_Handler;

    }

    void ResetLevelEvent_Handler()
    {
        foreach (var heart in  hearts)
        {
            heart.SetActive(true);
            Start();
        }
    }
    
    void PlayerLosedHpEvent_Handler(int hp)
    {
        LoseHeart();
    }
}