using System;
using UnityEngine;

public class Wallet
{
    public Wallet(string id)
    {
        this.id = id;
        amount = PlayerPrefs.GetInt(Key, 0);
    }
    
    readonly string id;
    int amount;

    public event Action<int> AmountChanged;
    
    public int Amount
    {
        get => amount;
        set
        {
            value = Mathf.Max(0, value);

            if(amount == value)
                return;
                
            amount = value;
            
            AmountChanged?.Invoke(amount);
            
            Save();
        }
    }

    public void Add(int income)
    {
        Amount += income;
    }
    
    public bool TryRemove(int outcome)
    {
        if (Has(outcome))
        {
            Amount -= outcome;
            return true;
        }

        return false;
    }

    public bool Has(int hasAmount) => Amount >= hasAmount;

    string Key => $"{id}_wallet";

    void Save()
    {
        PlayerPrefs.SetInt(Key, Amount);
        PlayerPrefs.Save();
    }
    
}
