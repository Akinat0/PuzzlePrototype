using Abu.Tools;
using Abu.Tools.UI;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class CustomBlend : UIComponent
{
    private enum PhotoshopBlendMode
    {
        Darken = 0,
        Multiply = 1,
        ColorBurn = 2,
        LinearBurn = 3,
        DarkenColor = 4,
        Lighten = 5,
        Screen = 6,
        ColorDodge = 7,
        LinearDodge = 8,
        LighterColor = 9,
        Overlay = 10,
        SoftLight = 11,
        HardLight = 12,
        VividLight = 13,
        LinearLight = 14,
        PinLight = 15,
        HardLerp = 16,
        Difference = 17,
        Exclusion = 18,
        Subtract = 19,
        Divide = 20,
        Hue = 21,
        Color = 22,
        Saturation = 23,
        Luminosity = 24
    }
    
    static readonly int SourceID = Shader.PropertyToID("_Source");
    static readonly int DestinationID = Shader.PropertyToID("_Destination");
    static readonly int ColorID = Shader.PropertyToID("_Color");
    static readonly int ModeID = Shader.PropertyToID("_BlendMode");
    
    [SerializeField] Camera Camera;
    [SerializeField] Texture UpperTexture;
    [SerializeField] Color UpperTextureColor;
    
    [Space(20)]
    
    [SerializeField] PhotoshopBlendMode BlendMode;

    Material BlendMaterial;
    MeshRenderer MeshRenderer;

    public Color TextureColor
    {
        get => UpperTextureColor;
        set
        {
            if (UpperTextureColor == value)
                return;

            UpperTextureColor = value;
            BlendMaterial.SetColor(ColorID, UpperTextureColor);
        }
    }
    
    void Awake()
    {
        Destroy(BlendMaterial);
        CreateMaterial();
    }
    
    void OnEnable()
    {
        ScreenScaler.FocusCameraOnBounds(MeshRenderer.bounds, Camera);
    }

    void CreateMaterial()
    {
        RenderTexture renderTexture = new RenderTexture(UpperTexture.width, UpperTexture.height, 16, RenderTextureFormat.ARGB32);

        Camera.forceIntoRenderTexture = true;
        Camera.targetTexture = renderTexture;
        
        BlendMaterial = new Material(Shader.Find("PhotoshopBlends"));
        
        MeshRenderer = GetComponent<MeshRenderer>();
        MeshRenderer.sharedMaterial = BlendMaterial;
        
        BlendMaterial.SetInt(ModeID, (int) BlendMode);
        BlendMaterial.SetTexture(SourceID, UpperTexture);
        BlendMaterial.SetTexture(DestinationID, renderTexture);
        BlendMaterial.SetColor(ColorID, UpperTextureColor);
    }


    protected override void OnValidate()
    {
        base.OnValidate();
        
        DestroyImmediate(BlendMaterial);
        CreateMaterial();
    }

    void OnDidApplyAnimationProperties()
    {
        ScreenScaler.FocusCameraOnBounds(MeshRenderer.bounds, Camera);
        BlendMaterial.SetColor(ColorID, UpperTextureColor);
    }

    [ContextMenu("Refresh")]
    void Refresh()
    {
        ScreenScaler.FocusCameraOnBounds(MeshRenderer.bounds, Camera);
        Camera.Render();
    }
    
}
