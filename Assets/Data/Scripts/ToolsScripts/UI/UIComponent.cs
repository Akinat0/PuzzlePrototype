
using UnityEngine;

namespace Abu.Tools.UI
{
    public abstract class UIComponent : MonoBehaviour
    {
        public void SetActive(bool value) => gameObject.SetActive(value);

        public void Destroy(float time = -1)
        {
            if(time < 0)
                DestroyImmediate(gameObject);
            else
                Destroy(gameObject, time);   
        }
        
        public bool ActiveInHierarchy => gameObject.activeInHierarchy;
        public bool ActiveSelf => gameObject.activeSelf;

        protected virtual void OnValidate() { }

        RectTransform rectTransform;
        public RectTransform RectTransform =>
            rectTransform ? rectTransform : rectTransform = transform as RectTransform;
    }
}