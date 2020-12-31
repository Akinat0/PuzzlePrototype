using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
public class Whirlpool : MonoBehaviour
{
    Material material;
    
    Material Material
    {
        get
        {
            if (material == null)
                material = GetComponent<MeshRenderer>().sharedMaterial;

            return material;
        }
    }

    [SerializeField] Color WaterColor = Color.cyan;
    [SerializeField] Color DeepWaterColor = Color.blue;
    [SerializeField] float Depth = 2.3f;
    [SerializeField] float WavesSpeed = 3;
    [SerializeField] float FoamSpeed = 1;
    [SerializeField] float FoamSize = 0.4f;
    [SerializeField] Vector2 FunnelPosition = new Vector2(0.5f, 0.5f);
    [SerializeField] float FunnelScale = 1;

    [Space(10)]
    
    [SerializeField] bool RuntimeNoiseTexture;
    [SerializeField] Vector2 NoiseTextureSize = new Vector2(200, 200);
    [SerializeField] float NoiseScale = 1;

    static readonly int WaterColorID = Shader.PropertyToID("_WaterColor");
    static readonly int DeepWaterColorID = Shader.PropertyToID("_DeepWaterColor");
    static readonly int DepthID = Shader.PropertyToID("_Depth");
    static readonly int WavesSpeedID = Shader.PropertyToID("_WavesSpeed");
    static readonly int FoamSpeedID = Shader.PropertyToID("_FoamSpeed");
    static readonly int FoamSizeID = Shader.PropertyToID("_FoamSize");
    static readonly int FunnelPositionID = Shader.PropertyToID("_FunnelPosition");
    static readonly int FunnelScaleID = Shader.PropertyToID("_FunnelScale");

    static readonly int NoiseTexID = Shader.PropertyToID("_NoiseTex");

    void Awake()
    {
        if (!RuntimeNoiseTexture)
            return;
            
        Texture noiseTexture = Utility.CreatePerlinNoiseTexture((int) NoiseTextureSize.x, (int) NoiseTextureSize.y, NoiseScale);
        Material.SetTexture(NoiseTexID, noiseTexture);
    
    }

    void OnDidApplyAnimationProperties()
    {
        ApplyProperties();
    }

    void OnValidate()
    {
        ApplyProperties();
    }

    void ApplyProperties()
    {
        if(Material == null)
            return;
        
        Material.SetColor(WaterColorID, WaterColor);
        Material.SetColor(DeepWaterColorID, DeepWaterColor);
        Material.SetFloat(DepthID, Depth);
        Material.SetFloat(WavesSpeedID, WavesSpeed);
        Material.SetFloat(FoamSpeedID, FoamSpeed);
        Material.SetFloat(FoamSizeID, FoamSize);
        Material.SetVector(FunnelPositionID, FunnelPosition);
        Material.SetFloat(FunnelScaleID, FunnelScale);
    }
}
