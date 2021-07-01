using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameActionQueue : MonoBehaviour
{
    protected readonly List<GameAction> GameActions = new List<GameAction>();

    protected GameAction Current { get; set; }
    
    void Update()
    {
        Current?.Update();
    }

    public void Pop(GameAction gameAction)
    {
        if (gameAction != Current)
        {
            Debug.LogError($"[GameActionQueue] Game action {gameAction.GetType().Name} doesn't match");
            return;
        }

        Current = null;
        
        Queue<GameAction> subActions = gameAction.SubActions;
        foreach (GameAction action in subActions)
            GameActions.Insert(0, action);

        if(GameActions.Count == 0)
            return;

        Current = GameActions.First();
        GameActions.RemoveAt(0);
        Current.Start();
    }

    public virtual void Add(GameAction gameAction)
    {
        gameAction.Initialize(this);
        
        if (GameActions.Count == 0 && Current == null)
        {
            Current = gameAction;
            Current.Start();
            return;
        }

        GameActions.Add(gameAction);
    }
    public void Reset()
    {
        Current?.Abort();
        Current = null;
        foreach (GameAction gameAction in GameActions)
            gameAction.Dispose();
        GameActions.Clear();
    }
}
