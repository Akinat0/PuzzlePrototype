using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Account : MonoBehaviour
{
    private static Account instance;

    public static Action<int> BalanceChangedEvent;
    
    [SerializeField] private WalletManager WalletManager;

    private void Start()
    {
        instance = this;
    }

    public static void AddCoins(int amount)
    {
        instance.WalletManager.AddCoins(amount);
        InvokeBalanceChanged(instance.WalletManager.Coins);
    }

    private static void InvokeBalanceChanged(int balance)
    {
        Debug.Log($"[Account] BalanceChangedEvent invoked. New balance is {balance}.");
        BalanceChangedEvent?.Invoke(balance);
    }
    
}
