using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Account : MonoBehaviour
{
    private static Account instance;

    public static Action<int> BalanceChangedEvent;
    
    [SerializeField] private WalletManager WalletManager;
    [SerializeField] private CollectionManager CollectionManager;
    
    
    private void Awake()
    {
        instance = this;
    }

    #region Wallet

    public static int Coins => instance.WalletManager.Coins;
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
    
    #endregion

    #region Collection

    public static int CollectionDefaultItemId
    {
        get { return instance.CollectionManager.DefaultItemID;}
        set { instance.CollectionManager.DefaultItemID = value; }
    }

    public static CollectionItem CollectionDefaultItem => instance.CollectionManager.DefaultItem;

    public static CollectionItem[] CollectionItems => 
        instance != null ? instance.CollectionManager.CollectionItems : null;

    #endregion
    
}
