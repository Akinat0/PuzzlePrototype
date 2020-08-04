using System;
using UnityEngine;

namespace Abu.Tools.UI
{
    public class ToggleComponent : ButtonComponent
    {
        public event Action<bool> ToggleValueChanged;

        [SerializeField] public Color InactiveButtonColor = Color.white;
        [SerializeField] bool isOn;
        public virtual bool IsOn
        {
            get => isOn;
            set
            {
                if (IsOn == value)
                    return;

                isOn = value;
                ProcessToggleValueChanged();
            }
        }

        ImageSkinComponent toggleImage;
        protected virtual ImageSkinComponent ToggleImage
        {
            get
            {
                if (toggleImage == null)
                    toggleImage = GetComponentInChildren<ImageSkinComponent>();

                return toggleImage;
            }
        }

        protected virtual void ProcessToggleValueChanged()
        {
            ToggleImage.Index = IsOn ? 1 : 0;
            Color = IsOn ? ButtonColor : InactiveButtonColor;
            ToggleValueChanged?.Invoke(IsOn);
        }
        
        protected virtual void Awake()
        {
            OnClick += () => IsOn = !IsOn;
        }

        protected override void OnValidate()
        {
            //We won't call base.OnValidate() because
            //we handle the same behaviour in ProcessToggleValueChanged()
            ProcessToggleValueChanged();
        }
    }
}