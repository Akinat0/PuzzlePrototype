
using System;

namespace Puzzle
{
    public abstract class MainMenuUIState : IDisposable
    {
        MainMenuUIManager MainMenu { get; }
        
        protected MainMenuUIState(MainMenuUIManager mainMenu)
        {
            MainMenu = mainMenu;
        }

        public bool IsRunning => MainMenu.CurrentState == this;
        
        public abstract void Start();
        public abstract void Stop();

        protected void ChangeStateTo<TState>() where TState : MainMenuUIState
        {
            MainMenu.ChangeStateTo<TState>();
        }

        public virtual void Dispose() { }
    }
}