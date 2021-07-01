
using System;

public class Chest
{
    public Chest(Rarity rarity, IChestContentResolver defaultContentResolver)
    {
        Rarity = rarity;
        DefaultContentResolver = defaultContentResolver;

        string id = $"{Rarity.ToString().ToLowerInvariant()}_chest";

        Wallet = new Wallet(id);
    }
    
    public event Action<Reward[]> OnOpen;
    
    public event Action<int> OnAmountAdded; 
    public event Action<int> OnAmountChanged
    {
        add => Wallet.AmountChanged += value;
        remove => Wallet.AmountChanged -= value;
    }

    public int Count => Wallet.Amount;
    
    public readonly Rarity Rarity;
    readonly IChestContentResolver DefaultContentResolver;
    readonly Wallet Wallet;
    
    public Reward[] Open(IChestContentResolver contentResolver = null)
    {
        if (!Wallet.TryRemove(1))
            return null;

        IChestContentResolver resolver = contentResolver ?? DefaultContentResolver;

        Reward[] rewards = resolver.GetRewards();
        
        OnOpen?.Invoke(rewards);
        
        foreach (Reward reward in rewards)
            reward.Claim();

        return rewards;
    }

    public void Add(int amount)
    {
        Wallet.Add(amount);
        OnAmountAdded?.Invoke(amount);
    }
}
