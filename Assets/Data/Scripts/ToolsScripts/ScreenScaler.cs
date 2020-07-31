﻿
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Abu.Tools
{
    public static class ScreenScaler
    {
        static Camera mainCamera;
        public static Camera MainCamera
        {
            get
            {
                if(mainCamera == null)
                    mainCamera = Camera.main;

                return mainCamera;
            }
            set => mainCamera = value;
        }

        public static Vector2 CameraSize
        {
            get{
                
                if (MainCamera == null)
                    return Vector2.one;

                float width = MainCamera.aspect * 2f * MainCamera.orthographicSize;
                float height = 2f * MainCamera.orthographicSize;

                return new Vector2(width, height);
            }
        }

        public static Vector2 ScreenSize => new Vector2(Screen.width, Screen.height);

        public static Vector3 WorldToScreenNormalized(Vector3 worldPosition)
        {
            Vector2 camSize = CameraSize;
            //Keep in mind that (zero,zero) of world is (camSize/2, camSize/2) of camera
            return new Vector2((worldPosition.x + camSize.x * 0.5f) / camSize.x, (worldPosition.y + camSize.y * 0.5f) / camSize.y);
        }
        
        public static Vector2 PartOfScreen(float part)
        {
            return CameraSize * part;
        }
        
        public static float BestFit(SpriteRenderer spriteRenderer)
        {
            float horizontalScale = FitHorizontal(spriteRenderer);
            float verticalScale = FitVertical(spriteRenderer);
            
            return Mathf.Abs(1 - horizontalScale) > Mathf.Abs(1 - verticalScale) ? verticalScale : horizontalScale;
        }
        
        public static float FitHorizontalPart(SpriteRenderer spriteRenderer, float part)
        {
            return FitHorizontal(spriteRenderer) * part;
        }
        
        public static float FitVerticalPart(SpriteRenderer spriteRenderer, float part)
        {
            return FitVertical(spriteRenderer) * part;
        }
        
        public static float FitHorizontal(SpriteRenderer spriteRenderer)
        {
            Sprite sprite = spriteRenderer.sprite;
            return ScaleToFillWidth(sprite.rect.width, sprite.pixelsPerUnit);
        }
        
        public static float FitVertical(SpriteRenderer spriteRenderer)
        {
            Sprite sprite = spriteRenderer.sprite;
            return ScaleToFillHeight(sprite.rect.height, sprite.pixelsPerUnit);
        }

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
        
        private static Vector2 ScaleToFillScreen (float width, float height, float pixelsPerUnit)
        {
            return new Vector2(ScaleToFillWidth(width, pixelsPerUnit), ScaleToFillHeight(height, pixelsPerUnit));
        }

        private static float ScaleToFillHeight(float height, float pixelsPerUnit)
        {
            float camHeight = pixelsPerUnit * MainCamera.orthographicSize * 2;
            float y_scale = camHeight / height;
            
            return y_scale;
        }
        
        private static float ScaleToFillWidth(float width, float pixelsPerUnit)
        {
            float aspectRatio = (float) Screen.width / Screen.height;

            float camHeight = pixelsPerUnit * MainCamera.orthographicSize * 2;
            float camWidth = camHeight * aspectRatio;
            
            float x_scale = camWidth / width;
            
            return x_scale;
        }
    }
}