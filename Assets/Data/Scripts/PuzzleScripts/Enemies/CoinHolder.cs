using System;
using Abu.Tools;
using Puzzle;
using UnityEngine;
using UnityEngine.Rendering;

public class CoinHolder : MonoBehaviour
{
    private int coins = 0;
    private GameObject particles;
    public int Coins => coins;
    
    void Start()
    {
        GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/CoinHolderMaterial");
        particles = Instantiate(Resources.Load<GameObject>("Prefabs/CoinParticles"),
            transform.position,
            transform.rotation,
            GameSceneManager.Instance.GameSceneRoot);
        
        particles.AddComponent<FollowerComponent>().Target = transform;
    }

    public void SetupCoinHolder(int amount)
    {
        coins = amount;
    }
    private void OnDestroy()
    {
        particles.AddComponent<SelfDestroy>().destroyTime = 5.0f;
    }

    
}