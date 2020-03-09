using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Account : MonoBehaviour
{
    private static Account instance;

    [SerializeField] private WalletManager WalletManager;

    private void Start()
    {
        instance = this;
    }

    public static void AddCoins(int amount)
    {
        instance.WalletManager.AddCoins(amount);
    }
    
}
