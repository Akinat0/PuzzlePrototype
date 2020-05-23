using System;
using UnityEngine;

namespace Abu.Tools.SceneTransition
{
    public interface ISceneTransition
    {
        bool IsLoaded { get; }
        
        Transform Load();
        void Unload();

        void InTransition(float Time, Action OnFinish = null);
        void OutTransition(float Time, Action OnFinish = null);
    }
}