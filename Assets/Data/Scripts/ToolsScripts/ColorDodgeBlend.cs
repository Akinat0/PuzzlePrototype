using System;
using Abu.Tools;
using Abu.Tools.UI;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ColorDodgeBlend : UIComponent
{
    static readonly int SourceID = Shader.PropertyToID("_Source");
    static readonly int DestinationID = Shader.PropertyToID("_Destination");
    static readonly int ColorID = Shader.PropertyToID("_Color");
    
    [SerializeField] Camera Camera;
    [SerializeField] Texture UpperTexture;
    [SerializeField] Color UpperTextureColor;
    [SerializeField] Material Material;

    MeshRenderer MeshRenderer;

    public Color TextureColor
    {
        get => UpperTextureColor;
        set
        {
            if (UpperTextureColor == value)
                return;

            UpperTextureColor = value;
            Material.SetColor(ColorID, UpperTextureColor);
        }
    }
    
    void Awake()
    {
        RefreshMaterial();
    }
    
    void OnEnable()
    {
        ScreenScaler.FocusCameraOnBounds(MeshRenderer.bounds, Camera);
    }

    void RefreshMaterial()
    {
#if UNITY_EDITOR
        Material = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>("Assets/Data/Materials/Common/ColorDodgeMaterial.mat");
#endif
        
        RenderTexture renderTexture = new RenderTexture(UpperTexture.width, UpperTexture.height, 16, RenderTextureFormat.ARGB32);

        Camera.forceIntoRenderTexture = true;
        Camera.targetTexture = renderTexture;

        MeshRenderer = GetComponent<MeshRenderer>();
        MeshRenderer.sharedMaterial = Material;
        
        Material.SetTexture(SourceID, UpperTexture);
        Material.SetTexture(DestinationID, renderTexture);
        Material.SetColor(ColorID, UpperTextureColor);
    }


    protected override void OnValidate()
    {
        base.OnValidate();
        RefreshMaterial();
    }

    void OnDidApplyAnimationProperties()
    {
        ScreenScaler.FocusCameraOnBounds(MeshRenderer.bounds, Camera);
        Material.SetColor(ColorID, UpperTextureColor);
    }

    [ContextMenu("Refresh")]
    void Refresh()
    {
        RefreshMaterial();
        ScreenScaler.FocusCameraOnBounds(MeshRenderer.bounds, Camera);
        Camera.Render();
    }

}
