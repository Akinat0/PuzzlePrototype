using System;
using System.Linq;
using UnityEngine;

public class Account : MonoBehaviour
{
    private static Account instance;

    public static event Action<int> BalanceChangedEvent;

    WalletManager WalletManager
    {
        get
        {
            if (walletManager == null)
                walletManager = GetComponent<WalletManager>();
            return walletManager;
        }
    }
    CollectionManager CollectionManager
    {
        get
        {
            if (collectionManager == null)
                collectionManager = GetComponent<CollectionManager>();
            return collectionManager;
        }
    }
    LevelsManager LevelsManager
    {
        get
        {
            if (levelsManager == null)
                levelsManager = GetComponent<LevelsManager>();
            return levelsManager;
        }
    }
    public static Achievement[] Achievements => instance.achievements;

    Achievement[] achievements; 
    WalletManager walletManager;
    CollectionManager collectionManager;
    LevelsManager levelsManager;
    
    
    private void Awake()
    {
        instance = this;
        achievements = Achievement.CreateAllAchievements();
    }

    #region Achievement
    
    public static T GetAchievement<T>() where T : Achievement
    {
        return instance.achievements.FirstOrDefault(achievement => typeof(Achievement) == typeof(T)) as T;
    }

    #endregion
    
    #region Wallet

    public static int Coins => instance.WalletManager.Coins;

    public static void AddCoins(int amount)
    {
        instance.WalletManager.AddCoins(amount);
        InvokeBalanceChanged();
    }

    private static void InvokeBalanceChanged()
    {
        int balance = instance.WalletManager.Coins;
        Debug.Log($"[Account] BalanceChangedEvent invoked. New balance is {balance}.");
        BalanceChangedEvent?.Invoke(balance);
    }
    
    #endregion

    #region Collection

    public static int CollectionDefaultItemId
    {
        get => instance.CollectionManager.DefaultItemID;
        set => instance.CollectionManager.DefaultItemID = value;
    }

    public static CollectionItem CollectionDefaultItem => instance.CollectionManager.DefaultItem;

    public static CollectionItem[] CollectionItems => 
        instance != null ? instance.CollectionManager.CollectionItems : null;

    #endregion

    #region Levels

    public static int DefaultLevelId
    {
        get => instance.LevelsManager.DefaultItemID;
        set => instance.LevelsManager.DefaultItemID = value;
    }
    
    public static LevelConfig[] LevelConfigs =>
        instance != null ? instance.LevelsManager.LevelConfigs : null;
    
    public static LevelConfig DefaultLevel => instance.LevelsManager.DefaultLevel;


    #endregion
}
