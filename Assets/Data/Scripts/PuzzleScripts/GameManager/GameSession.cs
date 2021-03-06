using System.Collections.Generic;
using Puzzle;
using PuzzleScripts;
using UnityEngine;

public class GameSession
{
    public GameSession(LevelConfig levelConfig)
    {
        SessionNumber = PlayerPrefs.GetInt(SessionNumberKey, 0) + 1;
        PlayerPrefs.SetInt(SessionNumberKey, SessionNumber);
        PlayerPrefs.Save();
        
        LevelConfig = levelConfig;
        StartTime = Time.time;
        Puzzle = Account.CollectionDefaultItem;

        balanceOnStart = Account.Coins;
        
        GameSceneManager.ApplyBoosterEvent += ApplyBoosterEvent_Handler;
        GameSceneManager.PlayerReviveEvent += PlayerReviveEvent_Handler;
        GameSceneManager.PlayerLosedHpEvent += PlayerLosedHpEvent_Handler;
        GameSceneManager.LevelCompletedEvent += LevelCompletedEvent_Handler;
        GameSceneManager.LevelClosedEvent += LevelClosedEvent_Handler;
        Account.BalanceChangedEvent += BalanceChangedEvent_Handler;
    }

    protected static string SessionNumberKey => "game_session_number";
    
    public int SessionNumber { get; protected set; }
    public LevelConfig LevelConfig { get; protected set; }
    public string LevelName => LevelConfig.Name;
    public DifficultyLevel Difficulty => LevelConfig.DifficultyLevel;
    public HashSet<Booster> ActiveBoosters { get; protected set; } = new HashSet<Booster>();
    public List<EnemyParams> Enemies { get; protected set; } = new List<EnemyParams>();
    public int CollectedCoins { get; protected set; }
    
    public int TotalLives { get; protected set; }
    public int Lives { get; protected set; } = GameSceneManager.DEFAULT_HEARTS;
    public bool ReviveUsed { get; protected set; }
    public CollectionItem Puzzle { get; protected set; }
    public float StartTime { get; private set; }
    
    public float? EndTime { get; private set; } = null;
    public LevelResult? Result { get; private set; }

    public float CurrentLevelTime => EndTime.HasValue ? -1 : Time.time - StartTime;
    
    public int CurrentCombo { get; set; }
    
    public void IncrementCombo()
    {
        const float comboTimeout = 4;

        if (Time.time - lastPrefectHitTime > comboTimeout)
            CurrentCombo = 0;

        CurrentCombo++;
        lastPrefectHitTime = Time.time;
    }

    readonly int balanceOnStart;

    float lastPrefectHitTime = -1;

    void Complete()
    {
        GameSceneManager.ApplyBoosterEvent -= ApplyBoosterEvent_Handler;
        GameSceneManager.PlayerReviveEvent -= PlayerReviveEvent_Handler;
        GameSceneManager.PlayerLosedHpEvent -= PlayerLosedHpEvent_Handler;
        GameSceneManager.LevelCompletedEvent -= LevelCompletedEvent_Handler;
        GameSceneManager.LevelClosedEvent -= LevelClosedEvent_Handler;
        Account.BalanceChangedEvent -= BalanceChangedEvent_Handler;
    }

    void ApplyBoosterEvent_Handler(Booster booster)
    {
        ActiveBoosters.Add(booster);

        if (booster is HeartBooster)
        {
            TotalLives++;
            Lives++;
        }
    }

    void PlayerReviveEvent_Handler()
    {
        ReviveUsed = true;
        Lives = TotalLives;
    }

    void PlayerLosedHpEvent_Handler()
    {
        CurrentCombo = 0;
        Lives--;
    }
    
    void BalanceChangedEvent_Handler(int balance)
    {
        CollectedCoins = balance - balanceOnStart;
    }

    void LevelCompletedEvent_Handler(LevelCompletedEventArgs args)
    {
        Result = LevelResult.Completed;
        EndTime = Time.time;
        Complete();
    }

    void LevelClosedEvent_Handler()
    {
        Result = Result ?? LevelResult.Failed;
        Complete();
    }
    
    public enum LevelResult 
    {
        Completed = 0,
        Failed = 1
    }
}