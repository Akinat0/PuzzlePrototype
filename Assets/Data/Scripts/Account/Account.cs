﻿using System;
using System.Linq;
using Puzzle.Advertisements;
using UnityEngine;

public class Account : MonoBehaviour
{
    static Account instance;

    public static event Action<int> BalanceChangedEvent;

    WalletManager WalletManager
    {
        get
        {
            if (walletManager == null)
                walletManager = gameObject.AddComponent<WalletManager>();
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
    public static Achievement[] Achievements => achievements = achievements ?? Achievement.CreateAllAchievements();
    public static Booster[] Boosters => boosters = boosters ?? Booster.CreateAllBoosters();
    public static Tier[] Tiers => tiers = tiers ?? Tier.CreateAllTiers();
    public static RemoteConfig RemoteConfig => remoteConfig = remoteConfig ?? new RemoteConfig();
    public static PuzzleAnalytics Analytics => analytics = analytics ?? new PuzzleAnalytics();
    public static PuzzleAdvertisement Advertisement => advertisement = advertisement ?? new PuzzleAdvertisement();

    static RemoteConfig remoteConfig;
    static PuzzleAnalytics analytics;
    static PuzzleAdvertisement advertisement;
    
    static Achievement[] achievements;
    static Booster[] boosters;
    static Tier[] tiers;
    static WalletManager walletManager;
    static CollectionManager collectionManager;
    static LevelsManager levelsManager;

    void Awake()
    {
        instance = this;
    }
    
    #region Boosters

    public static T GetBooster<T>() where T : Booster
    {
        return Boosters.FirstOrDefault(booster => booster.GetType() == typeof(T)) as T;
    }
    
    public static Booster[] GetActiveBoosters()
    {
        return Boosters.Where(booster => booster.IsActivated).ToArray();
    } 
    
    #endregion

    #region Tiers

    public static Tier GetTier<T>() where T : Tier
    {
        return Tiers.FirstOrDefault(tier => tier.GetType() == typeof(T)) as T;
    }
    
    public static Tier GetTier(int id)
    {
        return Tiers.FirstOrDefault(tier => tier.ID == id);
    }
    
    #endregion
    
    #region Achievement
    
    public static T GetAchievement<T>() where T : Achievement
    {
        return Achievements.FirstOrDefault(achievement => achievement.GetType() == typeof(T)) as T;
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

    public static void UnlockCollectionItemColor(int ID, string colorID)
    {
        instance.CollectionManager.UnlockItemColor(ID, colorID);
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

    public static LevelConfig GetLevel(string levelName)
    {
        return LevelConfigs?.FirstOrDefault(level => level.Name == levelName);
    }
    
    #endregion
}
