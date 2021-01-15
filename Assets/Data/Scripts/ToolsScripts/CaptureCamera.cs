using Abu.Console;
using Abu.Tools.UI;
using UnityEngine;

namespace Abu.Tools
{
    public static class CaptureUtility
    { 
        public static Texture2D Capture(SpriteRenderer renderer, bool inOtherLayer = true)
        {
            int prevSpriteLayer = renderer.gameObject.layer;

            Vector2 spriteScale = new Vector2(renderer.transform.lossyScale.x, renderer.transform.lossyScale.y);

            Camera camera = CreateCameraForSprite(renderer);

            if (inOtherLayer)
            {
                renderer.gameObject.layer = LayerMask.NameToLayer("RenderTexture");
                camera.cullingMask = 1 << LayerMask.NameToLayer("RenderTexture");
            }

            int textureWidth = Mathf.CeilToInt(renderer.sprite.rect.width * spriteScale.x);
            int textureHeight = Mathf.CeilToInt(renderer.sprite.rect.height * spriteScale.y);
            
            RenderTexture renderTexture = RenderTexture.GetTemporary(textureWidth, textureHeight);
            
            camera.targetTexture = renderTexture;
            camera.Render();

            Texture2D texture = camera.targetTexture.ToTexture2D();
            
            Object.DestroyImmediate(camera.gameObject);
            RenderTexture.ReleaseTemporary(renderTexture);

            renderer.gameObject.layer = prevSpriteLayer;

            return texture;
        }

        public static Camera CreateCameraForSprite(SpriteRenderer renderer)
        {
            Rect spriteRect = ScreenScaler.SpriteRectInWorld(renderer);
            
            Camera camera = new GameObject(renderer.name + "_RenderCamera").AddComponent<Camera>();
            
            Vector2 spriteScale = new Vector2(renderer.transform.lossyScale.x, renderer.transform.lossyScale.y);
            
            camera.orthographic = true;
            camera.orthographicSize = spriteRect.height * spriteScale.y / 2;
            
            camera.transform.position = spriteRect.position;
            camera.transform.position += Vector3.back;

            camera.backgroundColor = Color.clear;
            camera.clearFlags = CameraClearFlags.SolidColor;

            return camera;
        }

        public static Camera CreateCameraForMesh(MeshRenderer mesh)
        {
            Bounds bounds = mesh.bounds;
            Vector2 meshScale = new Vector2(mesh.transform.lossyScale.x, mesh.transform.lossyScale.y);
            
            Camera camera = new GameObject(mesh.name + "_RenderCamera").AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = bounds.size.y * meshScale.y / 2;
            
            camera.transform.position = mesh.transform.position;
            camera.transform.position += Vector3.back;
            
            camera.backgroundColor = Color.clear;
            camera.clearFlags = CameraClearFlags.SolidColor;

            return camera;
        }

        public static Camera CreateCameraForScreen(Camera mainCamera)
        {
            Camera camera = new GameObject( "Screen_RenderCamera").AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = mainCamera.orthographicSize;
            
            camera.transform.position = mainCamera.transform.position;

            camera.backgroundColor = Color.clear;
            camera.clearFlags = CameraClearFlags.SolidColor;

            return camera;
        }
        
    }
}