using Abu.Tools.UI;
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

        public float Alpha
        {
            get => AlphaSelf;
            set
            {
                AlphaSelf = Mathf.Clamp01(value);
                UpdateColor();
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

        public TextMeshProUGUI Text
        {
            get
            {
                if (text == null)
                    text = GetComponentInChildren<TextMeshProUGUI>();

                return text;
            }
        }

        private TextMeshProUGUI text;
        private Image coinImage;

        protected override void OnValidate()
        {
            CoinImage.sprite = Sprite;
            UpdateColor();
        }

        void Start()
        {
            Text.text = Account.Coins.ToString();
        }

        void UpdateColor()
        {
            Color spriteColor = CoinImage.color;
            spriteColor.a = Alpha;
            CoinImage.color = spriteColor;

            Text.alpha = Alpha;

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

        protected virtual void OnEnable()
        {
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
            Text.text = amount.ToString();
            
            CoinImage.transform.DOKill();
            CoinImage.transform.localScale = Vector3.one;
            
            CoinImage.transform.DOPunchScale(new Vector3(1.1f, 1.1f, 1.1f), 0.3f, 2, 0.6f);
        }
        
        void OnLevelChangedHandler(LevelChangedEventArgs args)
        {
            Text.color = args.LevelConfig.ColorScheme.TextColorLauncher;
        }
    }
}
