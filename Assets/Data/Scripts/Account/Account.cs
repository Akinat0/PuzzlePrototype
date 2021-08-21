﻿using System;
using System.Linq;
using Abu.Tools;
using Puzzle.Advertisements;
using UnityEngine;

public class Account : MonoBehaviour
{
    static Account instance;

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
    
    public static Chest CommonChest => commonChest = commonChest ?? new Chest(Rarity.Common, new CommonChestContentResolver()); 
    public static Chest RareChest => rareChest = rareChest ?? new Chest(Rarity.Rare, new RareChestContentResolver()); 
    public static Chest EpicChest => epicChest = epicChest ?? new Chest(Rarity.Epic, new EpicChestContentResolver());
    public static Wallet CommonShards => commonShards = commonShards ?? new Wallet("common_shards");
    public static Wallet RareShards => rareShards = rareShards ?? new Wallet("rare_shards");
    public static Wallet EpicShards => epicShards = epicShards ?? new Wallet("epic_shards");
    public static Wallet Stars => stars = stars ?? new Wallet("stars");

    public static Trigger BoostersAvailable => boostersAvailable = boostersAvailable ?? new Trigger("booster_available"); 
    public static Trigger CollectionAvailable => collectionAvailable = collectionAvailable ?? new Trigger("collection_available"); 
    public static Trigger AchievementsAvailable => achievementsAvailable = achievementsAvailable ?? new Trigger("achievements_available"); 
    public static Trigger ShopAvailable => shopAvailable = shopAvailable ?? new Trigger("shop_available");

    static RemoteConfig remoteConfig;
    static PuzzleAnalytics analytics;
    static PuzzleAdvertisement advertisement;
    static Wallet stars;

    static Trigger boostersAvailable;
    static Trigger collectionAvailable;
    static Trigger achievementsAvailable;
    static Trigger shopAvailable;
    
    static Achievement[] achievements;
    static Booster[] boosters;
    static Tier[] tiers;
    static CollectionManager collectionManager;
    static LevelsManager levelsManager;

    static Chest commonChest;
    static Chest rareChest;
    static Chest epicChest;

    static Wallet commonShards;
    static Wallet rareShards;
    static Wallet epicShards;

    void Awake()
    {
        instance = this;
        advertisement = advertisement ?? new PuzzleAdvertisement();
    }

    #region Shards

    public static Wallet GetShards(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return CommonShards;
            case Rarity.Rare:
                return RareShards;
            case Rarity.Epic:
                return EpicShards;
            default:
                return null;
        }
    } 

    #endregion
    
    #region Chests

    public static Chest GetChest(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return CommonChest;
            case Rarity.Rare:
                return RareChest;
            case Rarity.Epic:
                return EpicChest;
            default:
                return null;
        }
    } 

    #endregion
    
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

    public static Tier GetTier(string id)
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
    
    #region Stars

    public static event Action<int> StarsAmountChanged
    {
        add => Stars.AmountChanged += value;
        remove => Stars.AmountChanged -= value;
    }
        
    public static void AddStars(int amount) => Stars.Add(amount);

    public static bool HasStars(int amount) => Stars.Has(amount);

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
