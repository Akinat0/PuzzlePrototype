
using System;
using UnityEngine;

namespace Abu.Tools.UI
{
    public abstract class UIComponent : MonoBehaviour
    {
        public void SetActive(bool enable)
        {
            gameObject.SetActive(enable);
        }

        public bool ActiveInHierarchy => gameObject.activeInHierarchy;
        public bool ActiveSelf => gameObject.activeSelf;
        
        protected abstract void OnValidate();
    }
}