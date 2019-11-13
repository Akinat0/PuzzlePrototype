
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Abu.Tools
{
    public static class ScreenScaler
    {
        //For quadratic only
        public static float ScaleToFillPartOfScreen(SpriteRenderer spriteRenderer, float part)
        {
            Vector2 scale = ScaleToFillScreen(spriteRenderer);
            if(part>1)
                Debug.LogError("Part should be less than one");
            scale *= part;
            //Base on the smaller side
            return scale.x < scale.y ? scale.x : scale.y;
        }
        
        
        public static Vector2 ScaleToFillScreen (SpriteRenderer spriteRenderer)
        {
            Sprite sprite = spriteRenderer.sprite;
            return ScaleToFillScreen(
                sprite.rect.width,
                sprite.rect.height,
                sprite.pixelsPerUnit);
        }
        
        public static Vector2 ScaleToFillScreen (float width, float height, float pixelsPerUnit)
        {
            return new Vector2(ScaleToFillWidth(width, pixelsPerUnit), ScaleToFillHeight(height, pixelsPerUnit));
        }

        public static float ScaleToFillHeight(float height, float pixelsPerUnit)
        {
            float camHeight = pixelsPerUnit * Camera.main.orthographicSize * 2;
            float y_scale = camHeight / height;
            
            return y_scale;
        }
        
        public static float ScaleToFillWidth(float width, float pixelsPerUnit)
        {
            float aspectRatio = (float) Screen.width / Screen.height;
            Camera.main.aspect = aspectRatio;
            
            float camHeight = pixelsPerUnit * Camera.main.orthographicSize * 2;
            float camWidth = camHeight * aspectRatio;
            
            float x_scale = camWidth / width;
            
            return x_scale;
        }
    }
}