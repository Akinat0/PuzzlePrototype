using System;
using UnityEngine;

namespace Abu.Tools.UI
{
    public class ToggleComponent : ButtonComponent
    {
        public event Action<bool> ToggleValueChanged;
        
        [SerializeField] bool isOn;
        protected virtual bool IsOn
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
            ToggleValueChanged?.Invoke(IsOn);
        }
        
        protected virtual void Awake()
        {
            OnClick += () => IsOn = !IsOn;
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            ProcessToggleValueChanged();
        }
    }
}