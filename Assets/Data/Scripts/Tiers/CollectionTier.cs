
using System;

public class CollectionTier : Tier
{
    public CollectionTier(string puzzleName)
    {
        CollectionItem item = Account.GetCollectionItem(puzzleName);
        
        PuzzleReward = new PuzzleReward(item);
        Purchase = new ShardsPurchase(item.Cost, item.Rarity);
        ID = $"collection_puzzle_{item.Name.ToLowerInvariant()}";
    }
    
    public CollectionTier(CollectionItem item)
    {
        PuzzleReward = new PuzzleReward(item);
        Purchase = new ShardsPurchase(item.Cost, item.Rarity);
        ID = $"collection_puzzle_{item.Name.ToLowerInvariant()}";
    }
    
    protected PuzzleReward PuzzleReward { get; }
    public override string ID { get; }
    public override Reward Reward => PuzzleReward;
    public override TierType Type => TierType.Puzzle;
    public override Purchase Purchase { get; }

    public override void Parse(TierInfo tierInfo)
    {
        throw new NotImplementedException();
    }
}
