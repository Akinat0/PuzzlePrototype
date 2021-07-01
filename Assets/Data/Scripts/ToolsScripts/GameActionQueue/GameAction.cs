using System;
using System.Collections;
using System.Collections.Generic;

public abstract class GameAction
{
    public Action OnComplete;

    protected GameActionQueue Queue { get; private set; }
    public readonly Queue<GameAction> SubActions = new Queue<GameAction>();

    public void Initialize(GameActionQueue queue)
    {
        this.Queue = queue;
    }
    public abstract void Start();
    public abstract void Update();
    public abstract void Abort();
    public abstract void Dispose();

    protected virtual void Add(GameAction action)
    {
        SubActions.Enqueue(action);
    } 
    
    protected virtual void Pop()
    {
        OnComplete?.Invoke();
        Queue.Pop(this);
    }

    protected void StartCoroutine(IEnumerator coroutine)
    {
        Queue.StartCoroutine(coroutine);
    }
    
    protected void StopCoroutine(IEnumerator coroutine)
    {
        Queue.StopCoroutine(coroutine);
    }
}
