using Abu.Console;
using UnityEngine;

namespace Abu.Tools
{
    public static class CaptureUtility
    { 
        public static Texture2D Capture(SpriteRenderer sprite)
        {
            int prevSpriteLayer = sprite.gameObject.layer;
            sprite.gameObject.layer = LayerMask.NameToLayer("RenderTexture");

            Rect spriteRect = ScreenScaler.SpriteRectInWorld(sprite);
            
            Camera camera = new GameObject().AddComponent<Camera>();
            
            camera.cullingMask = 1 << LayerMask.NameToLayer("RenderTexture");
            Vector2 spriteScale = new Vector2(sprite.transform.lossyScale.x, sprite.transform.lossyScale.y);
            
            camera.orthographic = true;
            camera.orthographicSize = spriteRect.height * spriteScale.y / 2;
            
            camera.transform.position = spriteRect.position;
            camera.transform.position += Vector3.back;

            camera.backgroundColor = Color.clear;
            
            camera.clearFlags = CameraClearFlags.SolidColor;
            
            int textureWidth = Mathf.CeilToInt(sprite.sprite.rect.width * spriteScale.x);
            int textureHeight = Mathf.CeilToInt(sprite.sprite.rect.height * spriteScale.y);
            
            RenderTexture renderTexture = RenderTexture.GetTemporary(textureWidth, textureHeight);
            
            camera.targetTexture = renderTexture;
            camera.Render();
            
            Texture2D texture = camera.targetTexture.ToTexture2D();
            
            Object.DestroyImmediate(camera.gameObject);
            RenderTexture.ReleaseTemporary(renderTexture);

            sprite.gameObject.layer = prevSpriteLayer;

            ConsoleView.DebugTexture = texture;
            
            return texture;
        }
    }
}