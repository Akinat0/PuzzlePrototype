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

        protected Image CoinImage
        {
            get
            {
                if (coinImage == null)
                    coinImage = GetComponentInChildren<Image>();

                return coinImage;
            }
        }

        protected TextMeshProUGUI Text
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
        }

        void Start()
        {
            Text.text = Account.Coins.ToString();
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
            CoinImage.transform.DOPunchScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f, 2);
        }
        
        void OnLevelChangedHandler(LevelChangedEventArgs args)
        {
            Text.color = args.LevelConfig.ColorScheme.TextColorLauncher;
        }
    }
}
