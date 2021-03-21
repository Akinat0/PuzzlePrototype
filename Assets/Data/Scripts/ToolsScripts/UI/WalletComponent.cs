using DG.Tweening;
using ScreensScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Abu.Tools.UI
{
    public class WalletComponent : UIComponent
    {
        [SerializeField] protected Sprite Sprite;
        [SerializeField, Range(0, 1)] protected float AlphaSelf = 1;
        [SerializeField] protected float Spacing = 5;
        
        RectTransform CoinImageTransform
        {
            get
            {
                if (coinImageTransform == null)
                    coinImageTransform = CoinImage.GetComponent<RectTransform>();
                
                return coinImageTransform;
            }
        }
        
        Image CoinImage
        {
            get
            {
                if (coinImage == null)
                    coinImage = GetComponentInChildren<Image>();

                return coinImage;
            }
        }
        
        public float Alpha
        {
            get => AlphaSelf;
            set
            {
                AlphaSelf = Mathf.Clamp01(value);
                UpdateColor();
            }
        }

        public TextComponent Text
        {
            get
            {
                if (text == null)
                    text = GetComponentInChildren<TextComponent>();

                return text;
            }
        }

        RectTransform coinImageTransform;
        TextComponent text;
        Image coinImage;

        protected override void OnValidate()
        {
            CoinImage.sprite = Sprite;
            UpdateColor();
//            ProcessWalletLayout();
        }

        void UpdateColor()
        {
            Color spriteColor = CoinImage.color;
            spriteColor.a = Alpha;
            CoinImage.color = spriteColor;

            Text.Alpha = Alpha;

            if (Alpha < Mathf.Epsilon)
            {
                CoinImage.enabled = false;
                Text.enabled = false;
            }
            else
            {
                CoinImage.enabled = true;
                Text.enabled = true;
            }
        }

        public void ForceUpdateLayout()
        {
            ProcessWalletLayout();
        }
        
        void ProcessWalletLayout()
        {
            Text.TextMesh.ForceMeshUpdate();
            float textWidth = Text.TextMesh.GetRenderedValues(false).x;
            CoinImageTransform.anchoredPosition = new Vector2(- Spacing - textWidth - CoinImageTransform.rect.width / 2, 0);
        }

        protected virtual void OnEnable()
        {
            Text.Text = Account.Coins.ToString();
            
            ProcessWalletLayout();
            LauncherUI.LevelChangedEvent += OnLevelChangedHandler;
            Account.BalanceChangedEvent += OnBalanceChangedHandler;
        }

        protected virtual void OnDisable()
        {
            LauncherUI.LevelChangedEvent -= OnLevelChangedHandler;
            Account.BalanceChangedEvent -= OnBalanceChangedHandler;
        }

        void OnBalanceChangedHandler(int amount)
        {
            Text.Text = amount.ToString();

            ProcessWalletLayout();            
            
            CoinImage.transform.DOKill();
            CoinImage.transform.localScale = Vector3.one;
            
            CoinImage.transform.DOPunchScale(new Vector3(1.1f, 1.1f, 1.1f), 0.3f, 2, 0.6f);
        }
        
        void OnLevelChangedHandler(LevelChangedEventArgs args)
        {
            Text.Color = args.LevelConfig.ColorScheme.TextColorLauncher;
        }
    }
}
