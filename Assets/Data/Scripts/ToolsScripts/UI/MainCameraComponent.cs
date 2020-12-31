using UnityEngine;

namespace Abu.Tools.UI
{
    [RequireComponent(typeof(Camera))]
    public class MainCameraComponent : UIComponent
    {
        Camera camera;
        
        void Awake()
        {
            camera = GetComponent<Camera>();
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
        
        public void Shake(){
        
        }
    }
}