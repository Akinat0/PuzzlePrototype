using System;

public class LauncherCallbackAction : LauncherAction
{
    Action Callback { get; }

    public LauncherCallbackAction(Action callback, LauncherActionOrder order) : base(order)
    {
        Callback = callback;
    }

    public override void Start()
    {
        Callback?.Invoke();
        Pop();
    }
}