using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new WalletData", menuName = "Account/WalletData", order = 51)]
public class WalletData : SaveableScriptableObject
{
    [SerializeField, Obsolete("Use it only for console or tests")] public int coins = 0;
    
    public int Coins => coins;

    public void AddCoins(int amount)
    {
        coins += amount;
        Debug.Log($"{amount} coins added to the wallet. Current balance is {coins}");
    }

    public override void LoadSettings()
    {
        base.LoadSettings();
        Debug.Log($"Wallet was loaded. Current amount of coins is {coins}");
    }
    
    public override void SaveSettings()
    {
        base.SaveSettings();
        Debug.Log($"Wallet was saved. Saved amount of coins is {coins}");
    }
}
