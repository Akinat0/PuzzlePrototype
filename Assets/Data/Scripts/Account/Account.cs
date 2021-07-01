using System;
using System.Linq;
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
    
    public static Chest CommonChest => commonChest = commonChest ?? new Chest(Rarity.Common, new CommonChestResolver()); 
    public static Chest RareChest => rareChest = rareChest ?? new Chest(Rarity.Rare, new CommonChestResolver()); 
    public static Chest EpicChest => epicChest = epicChest ?? new Chest(Rarity.Epic, new CommonChestResolver()); 
    public static Wallet Stars => stars = stars ?? new Wallet("stars"); 

    static RemoteConfig remoteConfig;
    static PuzzleAnalytics analytics;
    static PuzzleAdvertisement advertisement;
    static Wallet stars;
    
    static Achievement[] achievements;
    static Booster[] boosters;
    static Tier[] tiers;
    static CollectionManager collectionManager;
    static LevelsManager levelsManager;

    static Chest commonChest;
    static Chest rareChest;
    static Chest epicChest;

    void Awake()
    {
        instance = this;
        advertisement = advertisement ?? new PuzzleAdvertisement();
    }

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
