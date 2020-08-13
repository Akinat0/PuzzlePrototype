using System;
using System.Collections;
using Abu.Tools;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ScreenFreezeEffect : MonoBehaviour
{
    #region factory
    static ScreenFreezeEffect prefab;
    static ScreenFreezeEffect Prefab
    {
        get
        {
            if (prefab == null)
                prefab = Resources.Load<ScreenFreezeEffect>("Prefabs/FreezeScreenEffect");
            
            return prefab;
        }
    }
    
    public static ScreenFreezeEffect Create(Transform parent)
    {
        return Instantiate(Prefab, parent);
    }
    
    #endregion

    static readonly int PhaseID = Shader.PropertyToID("_Phase");
    static readonly int IntensityID = Shader.PropertyToID("_Intensity");
    static readonly int NoiseUvID = Shader.PropertyToID("_NoiseUV");
    
    SpriteRenderer spriteRenderer;
    SpriteRenderer SpriteRenderer
    {
        get
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();

            return spriteRenderer;
        }  
    }

    Material material;
    Material Material
    {
        get
        {
            if (material == null)
                material = SpriteRenderer.material;
            return material;
        }    
    }

    void Start()
    {
        Vector2 scale = ScreenScaler.ScaleToFillScreen(SpriteRenderer);
        
        transform.localScale = new Vector3(scale.x, scale.y, 1);
        
        //Avoid division to zero
        if(Math.Abs(scale.y) > Mathf.Epsilon)
            Material.SetVector(NoiseUvID, new Vector4(scale.x / scale.y, scale.y/scale.y));
        
        //Make independent from parent's scale
        transform.localScale = new Vector3(
            transform.localScale.x * transform.localScale.x / transform.lossyScale.x,
            transform.localScale.y * transform.localScale.y / transform.lossyScale.y,
            transform.localScale.z * transform.localScale.z / transform.lossyScale.z);
    }
    
    public void Show()
    {
        StopAllCoroutines();
        StartCoroutine(ShowRoutine());
    }

    public void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(HideRoutine());
    }
    
    IEnumerator ShowRoutine()
    {
        float time = 0;
        float duration = 1.5f;

        while (time < duration)
        {
            float phase = time / duration;
            Material.SetFloat(PhaseID, Mathf.Lerp(0, 0.4f, phase));
            Material.SetFloat(IntensityID, Mathf.Lerp(0, 0.5f, phase));
            time += Time.deltaTime;
            Debug.LogError($"Hide time {time}");
            yield return null;
        }
        
        Material.SetFloat(PhaseID, 0.4f);
        Material.SetFloat(IntensityID, 0.5f);
    }
    
    IEnumerator HideRoutine()
    {
        float time = 0;
        float duration = 1.5f;

        while (time < duration)
        {
            float phase = time / duration;
            Material.SetFloat(PhaseID, Mathf.Lerp(0.4f, 0, phase));
            Material.SetFloat(IntensityID, Mathf.Lerp(0.5f, 0, phase));
            time += Time.deltaTime;
            Debug.LogError($"Show time {time}");
            yield return null;
        }
        
        Material.SetFloat(PhaseID, 0);
        Material.SetFloat(IntensityID, 0);
    }
    
    
#if UNITY_EDITOR

    [ContextMenu("Show")]
    public void ShowEditor()
    {
        Show();
    }
    
    [ContextMenu("Hide")]
    public void HideEditor()
    {
        Hide();
    }
#endif
}
