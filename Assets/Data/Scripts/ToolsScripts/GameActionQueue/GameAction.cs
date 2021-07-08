using System;
using System.Collections;
using System.Collections.Generic;
using Abu.Tools;
using UnityEngine;


public abstract class GameAction
{
    public Action OnComplete;

    GameActionQueue Queue { get; set; }
    public readonly Queue<GameAction> SubActions = new Queue<GameAction>();

    public void Initialize(GameActionQueue queue)
    {
        Queue = queue;
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
        if(Queue != null) 
            Queue.Pop(this);
    }

    protected void StartCoroutine(IEnumerator coroutine)
    {
        CoroutineHelper.StartRoutine(coroutine);
    }
    
    protected void StopCoroutine(IEnumerator coroutine)
    {
        CoroutineHelper.StopRoutine(coroutine);
    }
}
