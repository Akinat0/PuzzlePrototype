using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Booster
{
    public static Booster[] CreateAllBoosters()
    {
        return new Booster[] {new TimeFreezeBooster(), };
    }

    protected Booster()
    {
        //Convert int to bool
        IsActivated = PlayerPrefs.GetInt(IsActivatedKey, 0) != 0;
        Amount = PlayerPrefs.GetInt(AmountKey, 0);
    }
    
    public event Action BoosterActivatedEvent;
    public event Action BoosterDeactivatedEvent;
    public event Action AmountChangedEvent;

    bool isActivated;
    
    public bool IsActivated
    {
        get => isActivated;
        private set
        {
            if (value == isActivated)
                return;
    
            isActivated = value;
            
            SaveAmountAndActive();
            
            if (isActivated)
                BoosterActivatedEvent?.Invoke();
            else
                BoosterDeactivatedEvent?.Invoke();
        }
    }

    int amount = 1;
    public int Amount
    {
        get => amount;
        set
        {
            if (amount == value)
                return;

            amount = value;
            SaveAmountAndActive();
            AmountChangedEvent?.Invoke();
        }
    }
    
    public bool Activate()
    {
        if (Amount <= 0)
            return false;
        
        IsActivated = true;
        return true;
    }

    public bool Deactivate()
    {
        IsActivated = false;
        return false;
    }

    public void Use()
    {
        Amount--;
    }

    string Key => Name;
    string AmountKey => Key + " Amount";
    string IsActivatedKey => Key + " IsActivated";

    void SaveAmountAndActive()
    {
        PlayerPrefs.SetInt(AmountKey, Amount);
        PlayerPrefs.SetInt(IsActivatedKey, IsActivated ? 1 : 0);
    }
    
    public abstract string Name { get; }
    public abstract void Apply();
    
}
