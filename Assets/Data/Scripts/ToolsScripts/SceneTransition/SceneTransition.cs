using System;
using Abu.Tools.SceneTransition;

public class SceneTransitions
{
    public static ISceneTransition Create<T>(Action onInTransition = null, Action onOutTransition = null) where T : ISceneTransition, new()
    {
        ISceneTransition transition = new T();
        transition.Load();
        return transition;
    }
    
    public static ISceneTransition Create(SceneTransitionType type, Action onInTransition = null, Action onOutTransition = null)
    {
        switch (type)
        {
            case SceneTransitionType.CircleTransition:
                return Create<CircleTransition>(onInTransition, onOutTransition);
            default:
                return null;
        }
    }
}

public enum SceneTransitionType
{
    CircleTransition = 0
} 
