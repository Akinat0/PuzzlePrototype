using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CoinSceneManager : MonoBehaviour
{

    [SerializeField] private Text coinText;

    private void Start()
    {
        Color textColor = coinText.color;
        textColor.a = 0;
        coinText.color = textColor;
    }

    private void OnEnable()
    {
        Account.BalanceChangedEvent += BalanceChangedEvent_Handler;
    }

    private void OnDisable()
    {
        Account.BalanceChangedEvent -= BalanceChangedEvent_Handler;
    }

    private void BalanceChangedEvent_Handler(int balance)
    {
        coinText.text = balance.ToString();
        
        coinText.DOKill();
        var fadeOutTween = coinText.DOFade(1.0f, 1.0f);
        fadeOutTween.onComplete += () => coinText.DOFade(0.0f, 1.0f);
    }
    
}
