using System;
using System.Collections.Generic;

public abstract class GameAction
{
    public Action OnComplete;
    
    GameActionQueue queue;
    public readonly Queue<GameAction> SubActions = new Queue<GameAction>();

    public void Initialize(GameActionQueue queue)
    {
        this.queue = queue;
    }
    public abstract void Start();
    public abstract void Update();
    public abstract void Abort();

    protected virtual void Add(GameAction action)
    {
        SubActions.Enqueue(action);
    } 
    
    protected virtual void Pop()
    {
        OnComplete?.Invoke();
        queue.Pop(this);
    }
}
