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
                    blurMaterial = Resources.Load<Material>("Materials/BlurMaterial");
                
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
                    blurImage = GetComponentInChildren<RawImage>();

                return blurImage;
            }
        }

        RenderTexture blurTexture;

        public RenderTexture BlurTexture
        {
            get
            {
                if (blurTexture == null)
                {
                    blurTexture = new RenderTexture(Screen.width >> downRes, Screen.height >> downRes, 16,
                        RenderTextureFormat.ARGB32);
                    blurTexture.Create();
                }

                return blurTexture;
            }
        }

        protected virtual void Awake()
        {
            BlurImage.texture = BlurTexture;
        }

        public void RecreateBlurTexture()
        {
            LauncherUI.Instance.MainCamera.RenderIntoTexture(BlurTexture); 
            
            RenderTexture temporary = RenderTexture.GetTemporary(BlurTexture.width, blurTexture.height, 16);
            
            //We will use 2 iterations blur
            Graphics.Blit(BlurTexture, temporary, BlurMaterial);
            Graphics.Blit(temporary, BlurTexture, BlurMaterial);
            
            RenderTexture.ReleaseTemporary(temporary);
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