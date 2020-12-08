using System;
using System.Collections;
using System.IO;
using Abu.Tools;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public static class Utility 
{
    //Monobehaviour
    public static void Invoke(this MonoBehaviour _monoBehaviour, Action _action, float _time)
    {
        _monoBehaviour.StartCoroutine(CallInTime(_action, _time));
    }

    public static IEnumerator CallInTime(Action _action, float _time)
    {
        yield return new WaitForSeconds(_time);
        
        _action?.Invoke();
    }

    //Float
    public static float Remap (this float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
    
    public static int Remap (this int value, int from1, int to1, int from2, int to2)
    {
        float _value = value;
        return Mathf.RoundToInt(_value.Remap(from1, to1, from2, to2));
    }
    
    //Coroutines
    public static IEnumerator WaitUntil(Func<bool> predicate, Action finish)
    {
        yield return new WaitUntil(predicate);
        finish?.Invoke();
    }

    public static IEnumerator WaitFrames(int frames, Action finish)
    {
        int count = 0;

        while (count < frames)
        {
            yield return null;
            count++;
        }
        
        finish?.Invoke();
    }
    
    //Colors
    public static void SetAlpha(this TextMeshProUGUI textMesh, float value)
    {
        Color color = textMesh.color;
        color.a = value;
        textMesh.color = color;
    }
    
    //Transform
    public static void SetX(this Transform transform, float value)
    {
        Vector3 position = transform.position;
        position.x = value;
        transform.position = position;
    }
    
    public static void SetY(this Transform transform, float value)
    {
        Vector3 position = transform.position;
        position.y = value;
        transform.position = position;
    }
    
    public static void SetZ(this Transform transform, float value)
    {
        Vector3 position = transform.position;
        position.z = value;
        transform.position = position;
    }
    
    public static void SetIndependentScale(this Transform transform)
    {
        transform.localScale = new Vector3(
            transform.localScale.x * transform.localScale.x / transform.lossyScale.x,
            transform.localScale.y * transform.localScale.y / transform.lossyScale.y,
            transform.localScale.z * transform.localScale.z / transform.lossyScale.z);
    }
    
    //RectTransform
    public static bool IsVisibleOnTheScreen(this RectTransform rt)
    {
        switch (rt.GetComponentInParent<Canvas>().renderMode)
        {
            case RenderMode.ScreenSpaceCamera:
                return rt.IsVisibleOnTheScreenCamera();
            case RenderMode.ScreenSpaceOverlay:
                return rt.IsVisibleOnTheScreenOverlay();
            default:
                return rt.IsVisibleOnTheScreenOverlay();
        }
    }

    public static bool IsVisibleOnTheScreenCamera(this RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners (corners);
 
        float maxY = Mathf.Max (corners [0].y, corners [1].y, corners [2].y, corners [3].y);
        float minY = Mathf.Min (corners [0].y, corners [1].y, corners [2].y, corners [3].y);

        float maxX = Mathf.Max (corners [0].x, corners [1].x, corners [2].x, corners [3].x);
        float minX = Mathf.Min (corners [0].x, corners [1].x, corners [2].x, corners [3].x);

        Vector2 cameraSize = ScreenScaler.CameraSize;
        
        return maxY > -cameraSize.y / 2 && minY < cameraSize.y / 2 && maxX > -cameraSize.x / 2 && minX < cameraSize.x / 2;
    }
    
    public static bool IsVisibleOnTheScreenOverlay(this RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners (corners);
 
        float maxY = Mathf.Max (corners [0].y, corners [1].y, corners [2].y, corners [3].y);
        float minY = Mathf.Min (corners [0].y, corners [1].y, corners [2].y, corners [3].y);

        float maxX = Mathf.Max (corners [0].x, corners [1].x, corners [2].x, corners [3].x);
        float minX = Mathf.Min (corners [0].x, corners [1].x, corners [2].x, corners [3].x);

        Vector2 screenSize = ScreenScaler.ScreenSize;
        
        return maxY > 0 && minY < screenSize.y && maxX > 0 && minX < screenSize.x;
    }
    
    //Texture2D
    public static Texture2D Crop(this Texture2D texture, int width, int height)
    {
        float aspect = (float)width / height; 
        if (texture.width < width)
        {
            width = texture.width;
            height = (int) (1 / aspect * width);
        }
        
        if (texture.height < height)
        {
            height = texture.height;
            width = (int) (aspect * height);
        }

        Texture2D newTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        newTexture.SetPixels(texture.GetPixels(0, 0, width, height));
        newTexture.Apply();
        return newTexture;
    }
    
    //Render texture
    public static Texture2D ToTexture2D(
        this RenderTexture renderTexture,
        Texture2D targetTexture = null,
        TextureFormat? format = null
    )
    {
        if (targetTexture == null)
            targetTexture = new Texture2D(1, 1, format ?? TextureFormat.ARGB32, false);

        int width = renderTexture.width;
        int height = renderTexture.height;

        targetTexture.Resize(width, height, format ?? targetTexture.format, targetTexture.mipmapCount > 1);

        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = renderTexture;

        targetTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        RenderTexture.active = prev;

        targetTexture.Apply(false, false);

        return targetTexture;
    }
    
    //Perlin noise
    public static Texture2D CreatePerlinNoiseTexture(int width, int height, float scale)
    {        
        Texture2D noiseTex = new Texture2D(width, height);
        Color[] pixels = new Color[width * height];
        
        float y = 0;
        
        while (y < noiseTex.height)
        {
            float x = 0.0F;
            while (x < noiseTex.width)
            {
                //Here we suppose that height will be greater then width.
                //We are using it to be sure that texture will have the same scale on both coordinates.
                
                float xCoord = x / height * scale; 
                float yCoord = y / height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                pixels[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
                x++;
            }
            y++;
        }
        
        noiseTex.SetPixels(pixels);

        noiseTex.Apply();

        return noiseTex;
    }
    
    //Editor
    public static void SaveTextureAsPNG(Texture2D texture, string fullPath, bool force = false)
    {
        if(File.Exists(fullPath) && force)
            File.Delete(fullPath);
        
        byte[] bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(fullPath, bytes);
        Debug.Log(bytes.Length/1024  + "Kb was saved as: " + fullPath);
    }

}
