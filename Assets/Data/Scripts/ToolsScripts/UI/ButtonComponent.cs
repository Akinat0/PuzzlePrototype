using System;
using System.Linq;
using Data.Scripts.Tools.Input;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Abu.Tools.UI
{
    public class ButtonComponent : UIComponent
    {
        [SerializeField] protected AudioClip Sound;
        [SerializeField] protected Color ButtonColor = Color.white;
        
        public event Action OnClick;

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
                
                if(Sound != null)
                    SoundManager.Instance.PlayOneShot(Sound);
            }
        }

        protected override void OnValidate()
        {
            Color = ButtonColor;
        }
        
    }
}