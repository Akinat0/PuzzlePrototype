using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tier
{
    protected Tier()
    {
        Available = PlayerPrefs.GetInt(AvailableKey, 1) == 1;
    }

    public static Tier[] CreateAllTiers()
    {
        return new List<Tier>
        {
            new HeartBoosterTier(),
            
        }.ToArray();
    }

    public event Action<bool> OnAvailableChangedEvent;

    public abstract int ID { get; }
    public abstract Reward Reward { get; }
    public abstract Purchase Purchase { get; }

    string AvailableKey => ID + "Available";

    bool available;

    public bool Available
    {
        get => available;
        private set
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
}
