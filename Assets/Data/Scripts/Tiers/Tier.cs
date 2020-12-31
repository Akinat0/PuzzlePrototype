using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tier
{
    public enum TierType
    {
        Booster, 
        Puzzle
    }
    
    protected Tier()
    {
        Available = PlayerPrefs.GetInt(AvailableKey, 1) == 1;
    }

    public static Tier[] CreateAllTiers()
    {
        return new List<Tier>
        {
            new HeartBoosterTier(),
            new GreenPuzzleTier(),
            new RedPuzzleTier(),
            new TimeFreezeBoosterTier(),
            
        }.ToArray();
    }

    public event Action OnTierValueChangedEvent;
    public event Action<bool> OnAvailableChangedEvent;

    public abstract int ID { get; }
    public abstract Reward Reward { get; }
    public abstract Purchase Purchase { get; }
    public abstract TierType Type { get; }
    public abstract void Parse(TierInfo tierInfo);

    string AvailableKey => ID + "Available";

    bool available;

    public bool Available
    {
        get => available;
        protected set
        {
            if (available == value)
                return;

            available = value;
            
            PlayerPrefs.SetInt(AvailableKey, available ? 1 : 0);
            PlayerPrefs.Save();
            
            OnAvailableChangedEvent?.Invoke(available);
        }
    }

    public bool Obtain()
    {
        if (!Available)
            return false;

        if (!Purchase.Process())
            return false;
        
        Reward.Claim();
        return true;
    }

    protected void InvokeTierValueChanged()
    {
        OnTierValueChangedEvent?.Invoke();
    }
}
