using System;
using UnityEngine;

public class WalletManager : MonoBehaviour
{
    [SerializeField] private WalletData WalletData;

    public int Coins => WalletData.Coins;

    public void AddCoins(int amount)
    {
        WalletData.AddCoins(amount);
    }
    
    private void Awake()
    {
        WalletData.LoadSettings();
    }

    private void OnApplicationQuit()
    {
        WalletData.SaveSettings();
    }
}
