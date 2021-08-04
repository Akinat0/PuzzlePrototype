using System;
using System.Collections.Generic;
using System.Linq;
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
        IEnumerable<Tier> collectionTiers =
            Account.CollectionItems.Where(item => !item.Unlocked).Select(item => new CollectionTier(item));

        IEnumerable<Tier> commonTiers = new List<Tier>
        {
            new HeartBoosterTier(),
            new TimeFreezeBoosterTier(),
        }; 
        
        return commonTiers.Concat(collectionTiers).ToArray();
    }

    public event Action OnTierValueChangedEvent;
    public event Action<bool> OnAvailableChangedEvent;

    public abstract string ID { get; }
    public abstract Reward Reward { get; }
    public abstract Purchase Purchase { get; }
    public abstract TierType Type { get; }
    public abstract void Parse(TierInfo tierInfo);

    string AvailableKey => $"tier_{ID}_available";

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
