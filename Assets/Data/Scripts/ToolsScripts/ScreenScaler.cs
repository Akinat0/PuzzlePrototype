
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

                return GetCameraSize(MainCamera);
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

        public static Rect SpriteRectInWorld(SpriteRenderer spriteRenderer)
        {
            float xPos = spriteRenderer.transform.position.x;
            float yPos = spriteRenderer.transform.position.y;

            float width = spriteRenderer.sprite.rect.width / spriteRenderer.sprite.pixelsPerUnit;
            float height = spriteRenderer.sprite.rect.height /  spriteRenderer.sprite.pixelsPerUnit;

            width *= spriteRenderer.transform.lossyScale.x;
            height *= spriteRenderer.transform.lossyScale.y;
            
            return new Rect(xPos, yPos, width, height);
        }
        
        public static float BestFit(SpriteRenderer spriteRenderer)
        {
            float horizontalScale = FitHorizontal(spriteRenderer);
            float verticalScale = FitVertical(spriteRenderer);
            
            return Mathf.Abs(1 - horizontalScale) > Mathf.Abs(1 - verticalScale) ? verticalScale : horizontalScale;
        }
        
        public static float FitHorizontalPart(SpriteRenderer spriteRenderer, float part)
        {
            float horizontalScale = FitHorizontal(spriteRenderer);
            return horizontalScale * part;
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

        public static Vector2 GetCameraSize(Camera camera)
        {
            float width = camera.aspect * 2f * camera.orthographicSize;
            float height = 2f * camera.orthographicSize;

            return new Vector2(width, height);
        } 
        
        public static Mesh GetMeshSizeOfScreen()
        {
            Mesh mesh = new Mesh();
            
            Vector3[] vertices = new Vector3[4];
            Vector2[] uv= new Vector2[4];
            int[] triangles = new int[6];

            Vector2 camSize = CameraSize;

            vertices[0] = new Vector2(-camSize.x / 2, -camSize.y / 2);
            vertices[1] = new Vector2(-camSize.x / 2, camSize.y / 2);
            vertices[2] = new Vector2(camSize.x / 2, camSize.y / 2);
            vertices[3] = new Vector2(camSize.x / 2, -camSize.y / 2);

            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            
            triangles[3] = 0;
            triangles[4] = 2;
            triangles[5] = 3;
            
            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(0, 1);
            uv[2] = new Vector2(1, 1);
            uv[3] = new Vector2(1, 0);

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            return mesh;
        }

        public static void FocusCameraOnBounds(Bounds bounds, Camera camera)
        {
            camera.transform.position = bounds.center;
            camera.orthographicSize = bounds.extents.x;
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