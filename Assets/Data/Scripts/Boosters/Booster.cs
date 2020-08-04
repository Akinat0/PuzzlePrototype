using System;
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
            
            SaveActive();
            
            if (isActivated)
                BoosterActivatedEvent?.Invoke();
            else
                BoosterDeactivatedEvent?.Invoke();
        }
    }

    int amount = 2;
    public int Amount
    {
        get => amount;
        set
        {
            if (amount == value)
                return;

            amount = value;
            SaveAmount();
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

    void SaveAmount()
    {
        PlayerPrefs.SetInt(AmountKey, Amount);
        PlayerPrefs.Save();
    }
    void SaveActive()
    {
        PlayerPrefs.SetInt(IsActivatedKey, IsActivated ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    public abstract string Name { get; }
    public abstract void Apply();
    
}
