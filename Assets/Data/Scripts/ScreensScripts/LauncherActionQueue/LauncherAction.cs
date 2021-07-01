

public abstract class LauncherAction : SortedAction
{
    protected LauncherAction(LauncherActionOrder order)
    {
        Order = order;
    }

    LauncherActionOrder Order { get; }

    public override void Update()
    {
        
    }

    public override void Abort()
    {
        
    }

    public override void Dispose()
    {
        
    }

    public override int CompareTo(GameAction other)
    {
        if (other is LauncherAction launcherAction)
            return Order.CompareTo(launcherAction.Order);
        
        return 0;
    }
}
