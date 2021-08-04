using System.Collections;
using UnityEngine;

namespace Abu.Tools.UI
{
    public abstract class WalletComponent : UIComponent
    {
        [SerializeField, Range(0, 1)] float AlphaSelf = 1;
        
        IEnumerator fadeRoutine; 

        public float Alpha
        {
            get => AlphaSelf;
            set
            {
                AlphaSelf = Mathf.Clamp01(value);
                UpdateColor();
            }
        }
        
        TextComponent text;
        public TextComponent Text
        {
            get
            {
                if (text == null)
                    text = GetComponentInChildren<TextComponent>();

                return text;
            }
        }

        public void Show()
        {
            if(fadeRoutine != null)
                StopCoroutine(fadeRoutine);

            StartCoroutine(fadeRoutine = Coroutines.LerpRoutine(AlphaGetter, AlphaSetter, 1, 0.2f));
        }
        
        public void Hide()
        {
            if(fadeRoutine != null)
                StopCoroutine(fadeRoutine);

            StartCoroutine(fadeRoutine = Coroutines.LerpRoutine(AlphaGetter, AlphaSetter, 0, 0.2f));
        }

        protected abstract Wallet Wallet { get; }
        protected abstract string IconText { get; }

        protected override void OnValidate()
        {
            UpdateColor();
            UpdateText();
        }
        
        protected virtual void OnEnable()
        {
            UpdateText();
            
            Wallet.AmountChanged += OnAmountChanged;
        }

        protected virtual void OnDisable()
        {
            Wallet.AmountChanged -= OnAmountChanged;
        }
        
        void UpdateColor()
        {
            Text.Alpha = Alpha;
            Text.enabled = !Mathf.Approximately(Alpha, 0);
        }

        void UpdateText()
        {
            Text.Text = $"{IconText}{Wallet.Amount.ToString()}";
        }

        void OnAmountChanged(int amount)
        {
            UpdateText();
        }
        
        float AlphaGetter() => Alpha;
        void AlphaSetter(float alpha) => Alpha = alpha;
    }
}