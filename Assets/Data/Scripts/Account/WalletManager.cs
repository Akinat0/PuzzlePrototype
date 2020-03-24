﻿using System;
using UnityEngine;

public class WalletManager : MonoBehaviour
{
    [SerializeField, Obsolete("Use it only for console or tests")] public WalletData WalletData;

    public int Coins => WalletData.Coins;

    public void AddCoins(int amount)
    {
        WalletData.AddCoins(amount);
    }
    
    private void Start()
    {
        WalletData.LoadSettings();
    }

    private void OnApplicationQuit()
    {
        WalletData.SaveSettings();
    }
}