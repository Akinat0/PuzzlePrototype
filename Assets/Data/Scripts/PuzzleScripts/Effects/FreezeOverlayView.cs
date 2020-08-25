using System;
using System.Collections;
using Abu.Tools;
using Abu.Tools.UI;
using UnityEngine;
using UnityEngine.UI;

public class FreezeOverlayView : OverlayView
{
    
    [SerializeField] AudioClip FreezeSound;

    static readonly int VignetteID = Shader.PropertyToID("_Vignette");
    
    MeshRenderer meshRenderer;
    MeshRenderer MeshRenderer
    {
        get
        {
            if (meshRenderer == null)
                CreateFxObject();

            return meshRenderer;
        }  
    }

    Material material;
    Material Material
    {
        get
        {
            if (material == null)
                material = Resources.Load<Material>("Materials/FrostMaterial");
            
            return material;
        }    
    }


    GameObject fxObject;
    GameObject FXObject
    {
        get
        {
            if(fxObject == null)
                CreateFxObject();
            
            return fxObject;
        }
    }
    
    RawImage freezeImage;
    protected RawImage FreezeImage
    {
        get
        {
            if (freezeImage == null)
                freezeImage = GetComponentInChildren<RawImage>();

            return freezeImage;
        }
    }

    RenderTexture renderTexture;
    Camera renderCamera;
    
    protected virtual void Awake()
    {
        Background.raycastTarget = false;
        
        renderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        renderCamera = CaptureUtility.CreateCameraForMesh(MeshRenderer);
        renderCamera.targetTexture = renderTexture;

        FreezeImage.texture = renderTexture;
    }
    
    public void Show(Action finished = null)
    {
        FXObject.SetActive(true);
        renderCamera.gameObject.SetActive(true);
        
        SoundManager.Instance.PlayOneShot(FreezeSound);

        finished += () =>
        {
            FXObject.SetActive(false);
            renderCamera.gameObject.SetActive(false);
        };
        
        ChangePhase(1, 2.4f, finished);
    }

    public void Hide(Action finished = null)
    {
        FXObject.SetActive(true);
        renderCamera.gameObject.SetActive(true);
        
        finished += () =>
        {
            FXObject.SetActive(false);
            renderCamera.gameObject.SetActive(false);
        };
        
        ChangePhase(0, 1, finished);
    }


    void CreateFxObject()
    {
        if (fxObject != null)
            return;

        fxObject = new GameObject("freezeFxObject");
        fxObject.layer = LayerMask.NameToLayer("RenderTexture");
        fxObject.transform.position = Vector3.down * 100000f;

        meshRenderer = FXObject.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = FXObject.AddComponent<MeshFilter>();
        meshFilter.mesh = ScreenScaler.GetMeshSizeOfScreen();
        Material.SetFloat(VignetteID, 0);
//        Material.SetTexture(NoiseTextureID, Utility.CreatePerlinNoiseTexture(Screen.width, Screen.height, 17));
        meshRenderer.material = Material;
    }
    
    protected override void ProcessPhase()
    {
        Material.SetFloat(VignetteID, Phase);
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
