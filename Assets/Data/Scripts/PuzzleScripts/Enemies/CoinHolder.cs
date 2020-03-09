using System;
using Puzzle;
using UnityEngine;
using UnityEngine.Rendering;

public class CoinHolder : MonoBehaviour
{
    private int coins = 0;

    public int Coins => coins;

    void Start()
    {
        GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/CoinHolderMaterial");
        Instantiate(Resources.Load<GameObject>("Prefabs/CoinParticles"), transform);
    }

    public void SetupCoinHolder(int amount)
    {
        coins = amount;
    }
}
