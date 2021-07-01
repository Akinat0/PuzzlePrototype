using System;

public class SortedActionQueue : GameActionQueue
{
    public override void Add(GameAction action)
    {
        SortedAction sortedAction = action as SortedAction;
        
        if (sortedAction == null || GameActions.Count == 0 && Current == null)
        {
            base.Add(action);
            return;
        }

        sortedAction.Initialize(this);
        
        int index = -1;
        for (int i = GameActions.Count - 1; i >= 0; i--)
        {
            if(sortedAction.CompareTo(GameActions[i]) <= 0)
                break;

            index = i;
        }

        if (index < 0)
            GameActions.Add(sortedAction);
        else
            GameActions.Insert(index, sortedAction);
    }
}

public abstract class SortedAction : GameAction, IComparable<GameAction>
{
    public abstract int CompareTo(GameAction other);
}