using System;
using UnityEngine;

public enum TutorialState
{
    NotStarted = 0,
    Started = 1,
    Completed = 2
}

public class TutorialDataModel<TTutorialAction> where TTutorialAction : TutorialAction, new()
{
    string Key { get; }

    TutorialState? state;

    public TutorialState State
    {
        get => state ?? (state = (TutorialState)PlayerPrefs.GetInt(Key, 0)).Value;
        set
        {
            if (State == value)
                return;

            state = value;
            PlayerPrefs.SetInt(Key, (int)State);
            
            StateChanged?.Invoke(State);
        }
    }

    public bool IsCompleted => State == TutorialState.Completed;

    public event Action<TutorialState> StateChanged; 
    
    public TutorialDataModel(string key)
    {
        Key = $"tutorial_{key}_data_model";
    }

    public void Start(Action completed = null)
    {
        State = TutorialState.Started;
        
        void Completed()
        {
            State = TutorialState.Completed;
            completed?.Invoke();
        }
        
        Tutorials.StartTutorial(this, Completed);
    }
}