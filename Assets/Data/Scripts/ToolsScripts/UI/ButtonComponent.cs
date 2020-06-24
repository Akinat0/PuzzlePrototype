using System;
using System.Linq;
using Data.Scripts.Tools.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Abu.Tools.UI
{
    public class ButtonComponent : UIComponent
    {

        [SerializeField] protected Color ButtonColor = Color.white;
        
        public event Action OnClick;

        public virtual bool Interactable
        {
            get => Button.interactable;
            set => Button.interactable = value;
        }

        protected virtual Color Color
        {
            get => Background.color;
            set => Background.color = value;
        }
        protected virtual Image Background
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

        
        private Image background;
        private Button button;

        public void OnButtonClick()
        {
            //Interactable condition is already into account
            if (MobileInput.Condition)
                OnClick?.Invoke();
        }

        protected override void OnValidate()
        {
            Color = ButtonColor;
        }
        
    }
}