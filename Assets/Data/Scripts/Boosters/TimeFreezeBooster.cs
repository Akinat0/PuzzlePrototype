
using Puzzle;

public class TimeFreezeBooster : Booster
{
    public override string Name => "Time Freeze";

    public override void Apply()
    {
        TimeManager.DefaultTimeScale = 0.87f;
        
    }

}
