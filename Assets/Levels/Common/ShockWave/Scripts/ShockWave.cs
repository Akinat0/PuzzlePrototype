using Abu.Tools;
using Puzzle;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(Animator))]
public class ShockWave : MonoBehaviour
{
    static readonly int WeakComboID = Animator.StringToHash("WeakCombo");
    static readonly int MediumComboID = Animator.StringToHash("MediumCombo");
    static readonly int StrongComboID = Animator.StringToHash("StrongCombo");

    MeshRenderer meshRenderer;
    Camera renderCamera;
    RenderTexture renderTexture;
    Animator animator;

    AnimationEventBehaviour defaultState;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        animator = GetComponent<Animator>();
        defaultState = AnimationEventBehaviour.FindState(animator, "default");

        defaultState.OnEnter += DeactivateCamera;
        defaultState.OnExit += ActivateCamera;
    }

    void Initialize()
    {
        if (renderCamera == null)
        {
            renderCamera = CaptureUtility.CreateCameraForMesh(meshRenderer);
            renderCamera.cullingMask = 1 << LayerMask.NameToLayer(RenderLayer.LevelBackground);
            Transform cameraTransform = renderCamera.transform;
            cameraTransform.parent = transform;
        }

        int textureWidth = (int) ScreenScaler.ScreenSize.x;
        renderTexture = new RenderTexture(textureWidth, textureWidth, 16);
        
        renderCamera.targetTexture = renderTexture;
        meshRenderer.material.mainTexture = renderTexture;
        
        renderCamera.enabled = true;
        renderCamera.Render();
    }

    void Uninitialize()
    {
        renderCamera.enabled = false;
        
        renderTexture.Release();
        renderTexture = null;
    }

    void ActivateCamera()
    {
        if(renderCamera != null)
            renderCamera.enabled = true;
    }
    
    void DeactivateCamera()
    {
        if(renderCamera != null)
            renderCamera.enabled = false;
    }

    void PlayAnimation()
    {
        int combo = GameSceneManager.Instance.Session.CurrentCombo;

        if (combo > 20)
            animator.SetTrigger(StrongComboID);
        else if (combo > 7)
            animator.SetTrigger(MediumComboID);
        else
            animator.SetTrigger(WeakComboID);
    }
    
    void OnEnable()
    {
        GameSceneManager.SetupLevelEvent += SetupLevelEvent_Handler;
        GameSceneManager.LevelClosedEvent += LevelClosedEvent_Handler;
        GameSceneManager.PerfectKillEvent += PerfectKillEvent_Handler;
    }
    
    void OnDisable()
    {
        GameSceneManager.SetupLevelEvent -= SetupLevelEvent_Handler;
        GameSceneManager.LevelClosedEvent -= LevelClosedEvent_Handler;
        GameSceneManager.PerfectKillEvent -= PerfectKillEvent_Handler;
    }

    void SetupLevelEvent_Handler(LevelColorScheme _)
    {
        Initialize();
    }

    void LevelClosedEvent_Handler()
    {
        Uninitialize();
    }

    void PerfectKillEvent_Handler()
    {
        PlayAnimation();
    }
    
}
