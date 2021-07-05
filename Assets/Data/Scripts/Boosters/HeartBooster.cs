
using Puzzle;

public class HeartBooster : Booster
{
    public override string Name => "Heart";
    public override Rarity Rarity => Rarity.Common;

    protected override void Apply()
    {
        GameSceneManager.Instance.CurrentHearts++;
    }

    public override void Release()
    {
    }
}
