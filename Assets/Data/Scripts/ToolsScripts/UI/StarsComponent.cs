using ScreensScripts;
using UnityEngine;

namespace Abu.Tools.UI
{
    public class StarsComponent : UIComponent
    {
        [SerializeField, Range(0, 1)] float AlphaSelf = 1;

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
        
        protected override void OnValidate()
        {
            UpdateColor();
        }

        void UpdateColor()
        {
            Text.Alpha = Alpha;
            Text.enabled = !(Alpha < Mathf.Epsilon);
        }

        void UpdateText()
        {
            Text.Text = $"{EmojiHelper.StarEmoji}{Account.Stars.Amount.ToString()}";
        }

        void OnEnable()
        {
            UpdateText();
            
            LauncherUI.LevelChangedEvent += OnLevelChangedHandler;
            Account.StarsAmountChanged += OnStarsAmountChangedHandler;
        }

        void OnDisable()
        {
            LauncherUI.LevelChangedEvent -= OnLevelChangedHandler;
            Account.StarsAmountChanged -= OnStarsAmountChangedHandler;
        }

        void OnStarsAmountChangedHandler(int amount)
        {
            UpdateText();
        }
        
        void OnLevelChangedHandler(LevelChangedEventArgs args)
        {
            Text.Color = args.LevelConfig.ColorScheme.TextColorLauncher;
        }
    }
}
