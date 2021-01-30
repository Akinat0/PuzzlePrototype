using System.Collections.Generic;
using UnityEngine;

public class GameActionQueue : MonoBehaviour
{
    readonly LinkedList<GameAction> GameActions = new LinkedList<GameAction>();

    GameAction Current { get; set; }
    
    void Update()
    {
        Current?.Update();
    }

    public void Pop(GameAction gameAction)
    {
        // Debug.LogWarning($"[GameActionQueue] Game Action {gameAction.GetType().Name} poped");
        
        if (gameAction != Current)
        {
            Debug.LogError($"[GameActionQueue] Game action {gameAction.GetType().Name} doesn't match");
            return;
        }

        Current = null;
        
        Queue<GameAction> subActions = gameAction.SubActions;
        foreach (GameAction action in subActions)
            GameActions.AddFirst(action);
        
        
        if(GameActions.Count == 0)
            return;

        Current = GameActions.First.Value;
        GameActions.RemoveFirst();
        Current.Start();
    }

    public void Add(GameAction gameAction)
    {
        gameAction.Initialize(this);
        
        if (GameActions.Count == 0 && Current == null)
        {
            Current = gameAction;
            Current.Start();
            return;
        }

        GameActions.AddLast(gameAction);
    }
    public void Reset()
    {
        Current?.Abort();
        Current = null;
        GameActions.Clear();
    }
}
