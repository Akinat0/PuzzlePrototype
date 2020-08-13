using Abu.Console;
using ScreensScripts;
using UnityEngine;
using UnityEngine.UI;

namespace Abu.Tools.UI
{
    public class BlurOverlayView : OverlayView
    {
        [SerializeField] Color blurColor = Color.white;

        int downRes = 1;

        public Material blurMaterial;
        protected Material BlurMaterial
        {
            get
            {
                if (blurMaterial == null)
                {
                    blurMaterial = Resources.Load<Material>("Materials/BlurMaterial");
                    ConsoleView.Print("Blur material found " + (blurMaterial != null));
                }

                return blurMaterial;
            }
        }
        
        public Color BlurColor
        {
            get => blurColor;
            set
            {
                blurColor = value;
                BlurImage.color = BlurColor;
            }
        }

        RawImage blurImage;
        protected RawImage BlurImage
        {
            get
            {
                if (blurImage == null)
                {
                    blurImage = GetComponentInChildren<RawImage>();
                    ConsoleView.Print("Blur image found " + (blurImage != null));
                }

                return blurImage;
            }
        }
        
        RenderTexture renderTexture;
        
        protected virtual void Awake()
        {
            renderTexture = new RenderTexture(Screen.width >> downRes, Screen.height >> downRes, 16, RenderTextureFormat.ARGB32);
            renderTexture.Create();
            
            BlurImage.texture = renderTexture;
            ConsoleView.Print($"BlurOverlay awoke. Blur texture size {renderTexture.width} : {renderTexture.height}");
        }

        public void RecreateBlurTexture()
        {
            LauncherUI.Instance.MainCamera.RenderIntoTexture(renderTexture);
            
            ConsoleView.Print($"Blur texture recreated. Blur texture size {renderTexture.width} : {renderTexture.height}");
            
            //We will use 2 iterations blur
            Graphics.Blit(renderTexture, renderTexture, BlurMaterial);
            Graphics.Blit(renderTexture, renderTexture, BlurMaterial);

            ConsoleView.DebugTexture = renderTexture;
        }
        
        protected override void ProcessPhase()
        {
            Color color = BlurColor;
            color.a = Phase;
            BlurColor = color;

            bool isEnabled = Phase > Mathf.Epsilon;
            BlurImage.enabled = isEnabled;
            Background.enabled = isEnabled;

            bool isRaycastTarget = Mathf.Approximately(Phase, 1);

            if (isRaycastTarget)
            {
                ConsoleView.Print($"Became raycast target. Alpha is {BlurColor.a}" );
            }

            BlurImage.raycastTarget = isRaycastTarget;
            Background.raycastTarget = isRaycastTarget;
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            BlurImage.color = BlurColor;
        }
    }
}