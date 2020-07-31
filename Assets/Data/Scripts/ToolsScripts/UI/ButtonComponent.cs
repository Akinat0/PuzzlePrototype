using System;
using System.Linq;
using Data.Scripts.Tools.Input;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Abu.Tools.UI
{
    public class ButtonComponent : UIComponent, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected AudioClip Sound;
        [SerializeField] protected Color ButtonColor = Color.white;
        
        public event Action OnClick;
        public event Action OnHoldDown;
        public event Action OnHoldUp;
        public event Action OnHoldEnter;
        public event Action OnHoldExit;

        public virtual bool Interactable
        {
            get => Button.interactable;
            set => Button.interactable = value;
        }
        
        public virtual RectTransform RectTransform
        {
            get
            {
                if(rectTransform == null)
                    rectTransform = transform as RectTransform;
                return rectTransform;
            }
        }
        
        public virtual Color Color
        {
            get => Background.color;
            set => Background.color = value;
        }
        
        public virtual Image Background
        {
            get
            {
                if (background == null)
                    background = GetComponent<Image>();
                
                return background;
            }
        }
      
        protected virtual Button Button
        {
            get
            {
                if (button == null)
                    button = GetComponent<Button>();
                
                return button;
            }
        }

        private RectTransform rectTransform;
        private Image background;
        private Button button;
        
        public void OnButtonClick()
        {
            //Interactable condition is already into account
            if (MobileInput.Condition)
            {
                OnClick?.Invoke();
                
                if(Sound != null && SoundManager.Instance != null)
                    SoundManager.Instance.PlayOneShot(Sound);
            }
        }
        
        protected override void OnValidate()
        {
            Color = ButtonColor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnHoldDown?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnHoldUp?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnHoldEnter?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnHoldExit?.Invoke();
        }
    }
}