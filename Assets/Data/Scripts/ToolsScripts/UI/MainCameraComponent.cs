using UnityEngine;

namespace Abu.Tools.UI
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(Animator))]
    public class MainCameraComponent : UIComponent
    {
        static readonly int ShakeID = Animator.StringToHash("shake");
        
        Camera camera;
        Animator animator;
        
        void Awake()
        {
            camera = GetComponent<Camera>();
            animator = GetComponent<Animator>();
            ScreenScaler.MainCamera = camera;
        }

        public Camera Camera => camera;

        public void RenderIntoTexture(RenderTexture renderTexture)
        {
            Rect pixelRect = camera.pixelRect;
            RenderTexture cameraTexture = 
                RenderTexture.GetTemporary((int) pixelRect.width, (int) pixelRect.height);

            camera.targetTexture = cameraTexture;
            camera.Render();
            camera.targetTexture = null;
            
            Graphics.Blit(cameraTexture, renderTexture);
            
            RenderTexture.ReleaseTemporary(cameraTexture);
        }
        
        public void Shake()
        {
            animator.SetTrigger(ShakeID);
        }
    }
}