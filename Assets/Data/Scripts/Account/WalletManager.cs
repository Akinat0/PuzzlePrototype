using System;
using UnityEngine;

public class WalletManager : MonoBehaviour
{
    string Key => "Wallet";
    
    int coins;
    
    public int Coins
    {
        get => coins;
        set
        {
            coins = value;
            SaveCoins();
        }
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
    }
    
    private void Awake()
    {
        Coins = PlayerPrefs.GetInt(Key, 0);
    }

//    private void OnApplicationQuit()
//    {
//        Uncomment this
//        SaveCoins();
//    }

    void SaveCoins()
    {
        PlayerPrefs.SetInt(Key, Coins);
        PlayerPrefs.Save();
    }
}
