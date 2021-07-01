using Puzzle;
using ScreensScripts;
using UnityEngine;

public class LauncherActionQueue : MonoBehaviour
{
    [SerializeField] AchievementNotification achievementNotification;
    
    SortedActionQueue actionQueue;
    
    void OnEnable()
    {
        LauncherUI.GameEnvironmentLoadedEvent += GameEnvironmentLoadedEvent_Handler;
        LauncherUI.AchievementReceived        += AchievementReceived_Handler;
        
        Account.EpicChest.OnAmountAdded   += ChestEpicOnAmountAdded_Handler;
        Account.RareChest.OnAmountAdded   += ChestRareOnAmountAdded_Handler;
        Account.CommonChest.OnAmountAdded += ChestCommonOnAmountAdded_Handler;
    }

    void OnDisable()
    {
        LauncherUI.GameEnvironmentLoadedEvent -= GameEnvironmentLoadedEvent_Handler;
        LauncherUI.AchievementReceived        -= AchievementReceived_Handler;
        
        Account.EpicChest.OnAmountAdded   -= ChestEpicOnAmountAdded_Handler;
        Account.RareChest.OnAmountAdded   -= ChestRareOnAmountAdded_Handler;
        Account.CommonChest.OnAmountAdded -= ChestCommonOnAmountAdded_Handler;
    }

    void Awake()
    {
        actionQueue = gameObject.AddComponent<SortedActionQueue>();
        AddChestActions();
    }

    public void AddAction(LauncherAction launcherAction)
    {
        actionQueue.Add(launcherAction);
    }

    void AddChestActions()
    {
        int commonChestsCount = Account.CommonChest.Count;
        int rareChestsCount   = Account.RareChest.Count;
        int epicChestsCount   = Account.EpicChest.Count;

        for (int i = 0; i < epicChestsCount; i++)
            AddAction(new LauncherOpenChestAction(Rarity.Epic));
        
        for (int i = 0; i < rareChestsCount; i++)
            AddAction(new LauncherOpenChestAction(Rarity.Rare));
        
        for (int i = 0; i < commonChestsCount; i++)
            AddAction(new LauncherOpenChestAction(Rarity.Common));
    }

    void AchievementReceived_Handler(Achievement achievement)
    {
        AddAction(new LauncherAchievementAction(achievement, achievementNotification));
    }

    void GameEnvironmentLoadedEvent_Handler(GameSceneManager _)
    {
        AddAction(new LauncherBlockAction());
    }

    void ChestEpicOnAmountAdded_Handler(int amount)
    {
        for (int i = 0; i < amount; i++)
            AddAction(new LauncherOpenChestAction(Rarity.Epic));
    }
    
    void ChestRareOnAmountAdded_Handler(int amount)
    {
        for (int i = 0; i < amount; i++)
            AddAction(new LauncherOpenChestAction(Rarity.Rare));
    }
    
    void ChestCommonOnAmountAdded_Handler(int amount)
    {
        for (int i = 0; i < amount; i++)
            AddAction(new LauncherOpenChestAction(Rarity.Common));
    }
}