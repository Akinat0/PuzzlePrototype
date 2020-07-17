using System;
using Abu.Tools;
using Puzzle;
using UnityEngine;
using UnityEngine.Rendering;

public class CoinHolder : MonoBehaviour
{
    private int coins = 0;
    protected GameObject particles;
    protected GameObject sparkls;
    protected GameObject shine;
    protected SpriteMask mask;
    public int Coins => coins;
    
    void Start()
    {
        CreateEffects();
    }

    public void SetupCoinHolder(int amount)
    {
        coins = amount;
    }

    protected virtual void CreateEffects()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        particles = Instantiate(Resources.Load<GameObject>("Prefabs/CoinParticles"),
            transform.position,
            transform.rotation,
            GameSceneManager.Instance.GameSceneRoot);
        
        particles.transform.localScale = transform.localScale;
        particles.AddComponent<FollowerComponent>().Target = transform;

        mask = gameObject.AddComponent<SpriteMask>();
        mask.sprite = spriteRenderer.sprite;
        
        sparkls = Instantiate(Resources.Load<GameObject>("Prefabs/CoinSparkly"), transform);
        shine = Instantiate(Resources.Load<GameObject>("Prefabs/CoinShine"), transform);
    }
    
    private void OnDestroy()
    {
        particles.AddComponent<SelfDestroy>().destroyTime = 5.0f;
        Destroy(sparkls);
        Destroy(mask);
    }

    
}