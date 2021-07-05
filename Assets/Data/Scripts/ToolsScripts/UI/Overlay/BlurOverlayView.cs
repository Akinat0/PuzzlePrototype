using System;
using ScreensScripts;
using UnityEngine;
using UnityEngine.UI;

namespace Abu.Tools.UI
{
    public class BlurOverlayView : OverlayView
    {
        public enum BlurKernelSize
        {
            Small,
            Medium,
            Big
        }

        public enum RaycastTargetMode
        {
            OnZero,
            OnOne
        }

        #region serialized fields

        [SerializeField] Color blurColor = Color.white;

        [SerializeField, Range(0, 4)] int downRes = 1;
        
        [SerializeField, Range(0f, 1f)] public float interpolation = 1f;

        [SerializeField, Range(1, 8)] public int iterations = 1;

        [SerializeField] BlurKernelSize kernelSize = BlurKernelSize.Small;
        
        [SerializeField] bool gammaCorrection = true;

        [SerializeField] bool recreateWhileUpdate;

        [SerializeField] RaycastTargetMode raycastMode = RaycastTargetMode.OnOne;

        #endregion
        
        #region public properties
        
        protected override bool RaycastTarget
        {
            get => Background.raycastTarget && BlurImage.raycastTarget;
            set
            {
                Background.raycastTarget = value;
                BlurImage.raycastTarget = value;
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

        public int DownRes
        {
            get => downRes;
            set => downRes = Mathf.Clamp(value, 0, 4);
        }

        public float Interpolation
        {
            get => interpolation;
            set => interpolation = Mathf.Clamp(value, 0, 1);
        }

        public int Iterations
        {
            get => iterations;
            set => iterations = Mathf.Clamp(value, 1, 8);
        }

        public BlurKernelSize KernelSize
        {
            get => kernelSize;
            set => kernelSize = value;
        }

        public bool GammaCorrection
        {
            get => gammaCorrection;
            set => gammaCorrection = value;
        }

        public bool RecreateWhileUpdate
        {
            get => recreateWhileUpdate;
            set => recreateWhileUpdate = value;
        }

        public RaycastTargetMode RaycastMode
        {
            get => raycastMode;
            set
            {
                if(raycastMode == value)
                    return;

                raycastMode = value;
                ProcessRaycastTarget();
            }
        }
        
        #endregion
        
        #region private properties
        Material boxBlurMaterial;
        protected Material BoxBlurMaterial
        {
            get
            {
                if (boxBlurMaterial == null)
                    boxBlurMaterial = Resources.Load<Material>("Materials/BoxBlurMaterial");
                
                return boxBlurMaterial;
            }
        }

        
        Material gaussianBlurMaterial;
        public Material GaussianBlurMaterial
        {
            get
            {
                if (gaussianBlurMaterial == null)
                    gaussianBlurMaterial = Resources.Load<Material>("Materials/GaussianBlurMaterial");
                
                return gaussianBlurMaterial;
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
        RenderTexture BlurTexture
        {
            get
            {
                if (blurTexture == null)
                {
                    blurTexture = new RenderTexture(Screen.width, Screen.height, 16,
                        RenderTextureFormat.ARGB32);
                    blurTexture.Create();
                }

                return blurTexture;
            }
        }
        
        #endregion

        static readonly int Radius = Shader.PropertyToID("_Radius");
        
        protected virtual void Awake()
        {
            BlurImage.texture = BlurTexture;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            #if UNITY_EDITOR

            if (!Application.isPlaying)
                return;
            
            #endif
            
            BlurTexture.Release();
            Destroy(BlurTexture);
        }

        void RecreateBlurTexture()
        {
            LauncherUI.Instance.MainCamera.RenderIntoTexture(BlurTexture); 
            
            RenderTexture temporary = RenderTexture.GetTemporary(BlurTexture.width >> downRes, BlurTexture.height >> downRes, 16);
            
            //We will use box blur and then gaussian blur
            BoxBlur(BlurTexture, temporary);
            GaussianBlur(temporary, BlurTexture);

            RenderTexture.ReleaseTemporary(temporary);
        }

        public override void ChangePhase(float targetValue, float duration, Action finished = null)
        {
            base.ChangePhase(targetValue, duration, finished);
            
            if(targetValue >= Mathf.Epsilon)
                RecreateBlurTexture();    
        }
        
        protected override void ProcessPhase()
        {
            if(RecreateWhileUpdate)
                RecreateBlurTexture();
            
            Color color = BlurColor;
            color.a = Phase;
            BlurColor = color;

            bool isEnabled = Phase > Mathf.Epsilon;
            BlurImage.enabled = isEnabled;
            Background.enabled = isEnabled;
            
            ProcessRaycastTarget();
        }

        void ProcessRaycastTarget()
        {
            switch (RaycastMode)
            {
                case RaycastTargetMode.OnOne:
                    RaycastTarget = Mathf.Approximately(Phase, 1);
                    break;
                case RaycastTargetMode.OnZero:
                    RaycastTarget = Phase > Mathf.Epsilon;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            BlurImage.color = BlurColor;
            ProcessPhase();
        }

        protected void BoxBlur(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, BoxBlurMaterial);
        }
        protected void GaussianBlur (RenderTexture source, RenderTexture destination)
        {
            if (gammaCorrection)
            {
                Shader.EnableKeyword("GAMMA_CORRECTION");
            }
            else
            {
                Shader.DisableKeyword("GAMMA_CORRECTION");
            }

            int kernel = 0;

            switch (kernelSize)
            {
                case BlurKernelSize.Small:
                    kernel = 0;
                    break;
                case BlurKernelSize.Medium:
                    kernel = 2;
                    break;
                case BlurKernelSize.Big:
                    kernel = 4;
                    break;
            }

            RenderTexture rt2 = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);

            for (int i = 0; i < iterations; i++)
            {
                // helps to achieve a larger blur
                float radius = (float)i * interpolation + interpolation;
                GaussianBlurMaterial.SetFloat(Radius, radius);

                Graphics.Blit(source, rt2, GaussianBlurMaterial, 1 + kernel);
                source.DiscardContents();

                // is it a last iteration? If so, then blit to destination
                if (i == iterations - 1)
                {
                    Graphics.Blit(rt2, destination, GaussianBlurMaterial, 2 + kernel);
                }
                else
                {
                    Graphics.Blit(rt2, source, GaussianBlurMaterial, 2 + kernel);
                    rt2.DiscardContents();
                }
            }

            RenderTexture.ReleaseTemporary(rt2);
        }
    }
}