using DG.Tweening;
using ScreensScripts;
using UnityEngine;
using UnityEngine.UI;

namespace Abu.Tools.UI
{
    public class StarsComponent : UIComponent
    {
        [SerializeField] protected Sprite Sprite;
        [SerializeField, Range(0, 1)] protected float AlphaSelf = 1;
        [SerializeField] protected float Spacing = 5;
        
        RectTransform StarImageTransform
        {
            get
            {
                if (starImageTransform == null)
                    starImageTransform = StarImage.GetComponent<RectTransform>();
                
                return starImageTransform;
            }
        }
        
        Image StarImage
        {
            get
            {
                if (starImage == null)
                    starImage = GetComponentInChildren<Image>();

                return starImage;
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

        RectTransform starImageTransform;
        TextComponent text;
        Image starImage;

        protected override void OnValidate()
        {
            StarImage.sprite = Sprite;
            UpdateColor();
        }

        void UpdateColor()
        {
            Color spriteColor = StarImage.color;
            spriteColor.a = Alpha;
            StarImage.color = spriteColor;

            Text.Alpha = Alpha;

            if (Alpha < Mathf.Epsilon)
            {
                StarImage.enabled = false;
                Text.enabled = false;
            }
            else
            {
                StarImage.enabled = true;
                Text.enabled = true;
            }
        }

        void ProcessWalletLayout()
        {
            Text.TextMesh.ForceMeshUpdate();
            float textWidth = Text.TextMesh.GetRenderedValues(false).x;
            StarImageTransform.anchoredPosition = new Vector2(- Spacing - textWidth - StarImageTransform.rect.width / 2, 0);
        }

        protected virtual void OnEnable()
        {
            Text.Text = Account.Stars.Amount.ToString();
            
            ProcessWalletLayout();
            LauncherUI.LevelChangedEvent += OnLevelChangedHandler;
            Account.StarsAmountChanged += OnStarsAmountChangedHandler;
        }

        protected virtual void OnDisable()
        {
            LauncherUI.LevelChangedEvent -= OnLevelChangedHandler;
            Account.StarsAmountChanged -= OnStarsAmountChangedHandler;
        }

        void OnStarsAmountChangedHandler(int amount)
        {
            Text.Text = amount.ToString();

            ProcessWalletLayout();            
            
            StarImage.transform.DOKill();
            StarImage.transform.localScale = Vector3.one;
            
            StarImage.transform.DOPunchScale(new Vector3(1.1f, 1.1f, 1.1f), 0.3f, 2, 0.6f);
        }
        
        void OnLevelChangedHandler(LevelChangedEventArgs args)
        {
            Text.Color = args.LevelConfig.ColorScheme.TextColorLauncher;
        }
    }
}
