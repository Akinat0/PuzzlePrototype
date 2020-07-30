
using TMPro;
using UnityEngine;

namespace Abu.Tools.UI
{
    public abstract class UIComponent : MonoBehaviour
    {
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
        
        public bool ActiveInHierarchy => gameObject.activeInHierarchy;
        public bool ActiveSelf => gameObject.activeSelf;

        protected virtual void OnValidate() { }
    }
}