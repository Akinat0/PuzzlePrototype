using System;
using System.Collections.Generic;
using Puzzle.Analytics;
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

        void OnPurchaseSuccess()
        {
            Reward.Claim();
            SendTierObtainedAnalyticsEvent();
        }
            
        if (!Purchase.Process(OnPurchaseSuccess))
            return false;

        return true;
    }

    protected void InvokeTierValueChanged()
    {
        OnTierValueChangedEvent?.Invoke();
    }

    void SendTierObtainedAnalyticsEvent()
    {
        Dictionary<string, object> eventData = new Dictionary<string, object>()
        {
            {"tier_id", ID},
            {"tier_type", Type},
            {"stars", Account.Stars.Amount}
        };
        
        new SimpleAnalyticsEvent("tier_obtained", eventData).Send();
    }

}
