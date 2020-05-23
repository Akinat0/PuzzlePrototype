using System;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Abu.Tools.SceneTransition
{
    public class CircleTransition : ISceneTransition
    {
        private const string PathToTransitionPrefab = "Prefabs/CircleTransition";
        
        public Transform TransitionObject { get; private set; }

        public bool IsLoaded { get; private set; }

        float TargetScale => ScreenScaler.FitVertical(TransitionObject.GetComponent<SpriteRenderer>()) * 1.5f;

        public Transform Load()
        {
            if (IsLoaded)
                return TransitionObject;
            
            TransitionObject = Object.Instantiate(Resources.Load<GameObject>(PathToTransitionPrefab)).transform;
            TransitionObject.localScale = Vector3.zero;
            Object.DontDestroyOnLoad(TransitionObject);
            IsLoaded = true;
            return TransitionObject.transform;
        }

        public void Unload()
        {
            if (!IsLoaded)
                return;
            

            Object.Destroy(TransitionObject.gameObject);
            IsLoaded = false;
        }

        public void InTransition(float time = 0.8f, Action OnFinish = null)
        {
            if (!IsLoaded)
            {
                Debug.LogError("You're trying to call IN transition but it's not yet loaded");
                return;
            }

            TransitionObject.DOScale(TargetScale, time).SetUpdate(true).onComplete += () => OnFinish?.Invoke();
        }

        public void OutTransition(float time = 0.8f, Action OnFinish = null)
        {
            if (!IsLoaded)
            {
                Debug.LogError("You're trying to call OUT transition but it's not yet loaded");
                return;
            }

            TransitionObject.localScale = TargetScale * Vector3.one;
            TransitionObject.DOScale(0, time).SetUpdate(true).onComplete += () => OnFinish?.Invoke();
        }
        
    }
}