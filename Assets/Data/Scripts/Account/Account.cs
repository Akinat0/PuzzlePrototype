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
                walletManager = gameObject.GetComponent<WalletManager>();
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
    public static Booster[] Boosters => instance.boosters;
    public static Tier[] Tiers => instance.tiers;

    Achievement[] achievements;
    Booster[] boosters;
    Tier[] tiers;
    WalletManager walletManager;
    CollectionManager collectionManager;
    LevelsManager levelsManager;
    
    
    private void Awake()
    {
        instance = this;
        boosters = Booster.CreateAllBoosters();
        tiers = Tier.CreateAllTiers();
        achievements = Achievement.CreateAllAchievements();
    }
    
    #region Boosters

    public static T GetBooster<T>() where T : Booster
    {
        return instance.boosters.FirstOrDefault(booster => booster.GetType() == typeof(T)) as T;
    }
    
    public static Booster[] GetActiveBoosters()
    {
        return instance.boosters.Where(booster => booster.IsActivated).ToArray();
    } 
    
    #endregion

    #region Tiers

    public static Tier GetTier<T>() where T : Tier
    {
        return instance.tiers.FirstOrDefault(tier => tier.GetType() == typeof(T)) as T;
    }
    
    public static Tier GetTier(int id)
    {
        return instance.tiers.FirstOrDefault(tier => tier.ID == id);
    }
    
    #endregion
    
    #region Achievement
    
    public static T GetAchievement<T>() where T : Achievement
    {
        return instance.achievements.FirstOrDefault(achievement => achievement.GetType() == typeof(T)) as T;
    }

    #endregion
    
    #region Wallet

    public static int Coins => instance.WalletManager.Coins;

    public static void AddCoins(int amount)
    {
        instance.WalletManager.AddCoins(amount);
        InvokeBalanceChanged();
    }

    public static bool RemoveCoins(int amount)
    {
        if (Coins < amount)
            return false;
        
        instance.WalletManager.RemoveCoins(amount);
        InvokeBalanceChanged();
        return true;
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

    public static bool UnlockCollectionItem(int ID)
    {
        return instance.CollectionManager.UnlockItem(ID);
    }

    public static CollectionItem GetCollectionItem(string itemName)
    {
        return instance.CollectionManager.GetCollectionItem(itemName);
    }
    
    public static CollectionItem GetCollectionItem(int ID)
    {
        return instance.CollectionManager.GetCollectionItem(ID);
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
