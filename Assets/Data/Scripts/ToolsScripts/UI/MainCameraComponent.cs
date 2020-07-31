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
    }
}