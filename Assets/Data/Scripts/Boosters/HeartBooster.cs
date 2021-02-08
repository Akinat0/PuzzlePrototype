
using Puzzle;

public class HeartBooster : Booster
{
    public override string Name => "Heart";
    
    protected override void Apply()
    {
        GameSceneManager.Instance.CurrentHearts++;
    }

    public override void Release()
    {
    }
}
