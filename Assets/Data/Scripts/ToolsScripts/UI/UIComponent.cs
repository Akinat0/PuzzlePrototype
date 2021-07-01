
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

        RectTransform rectTransform;
        public RectTransform RectTransform =>
            rectTransform ? rectTransform : rectTransform = transform as RectTransform;
    }
}