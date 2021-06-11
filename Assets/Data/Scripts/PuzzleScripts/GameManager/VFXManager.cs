using System.Collections;
using Abu.Tools;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Puzzle
{
    public class VFXManager : MonoBehaviour
    {
        public static VFXManager Instance;
        
        FlatFX m_FlatFx;
        Coroutine m_LevelCompleteRoutine;
        GameObject m_TapVfx;
        GameObject m_TadaSFX;
        Volume m_Volume;

        public Vignette Vignette
        {
            get{
                m_Volume.profile.TryGet(out Vignette vignette);
                return vignette;
            }
        }

        public FlatFX FlatFx => m_FlatFx;

        void Awake()
        {
            Instance = this;
            m_FlatFx = GetComponent<FlatFX>();
            m_Volume = GetComponent<Volume>();
            m_TapVfx = Resources.Load<GameObject>("Prefabs/Tap");
            m_TadaSFX = Resources.Load<GameObject>("Prefabs/WinningSound");
        }

        public void StopLevelCompleteSunshineEffect()
        {
            FlatFx.particles.Clear();
        }

        public void CallCrosslightEffect(Vector2 position)
        {
            FlatFx.useUnscaledTime = false;
            FlatFx.AddEffect(position, (int)FlatFXType.Crosslight);
        }

        public void CallWinningSound()
        {
            Instantiate(m_TadaSFX);
        }
        
        public void CallTapRippleEffect(Vector3 position, float lifetime = 1, float startSize = 0, float endSize = 2, float startThickness = 1, float endThickness = 0, Color? startInnerColor = null, Color? startOuterColor = null, Color? endInnerColor = null, Color? endOuterColor = null)
        {
            FlatFx.useUnscaledTime = true;
            var settings = FlatFx.settings[(int) FlatFXType.Ripple];

            settings.lifetime = lifetime;
            settings.start.innerColor = startInnerColor ?? new Color(0.98f, 0.87f, 0.29f, 1);;
            settings.start.outerColor = startOuterColor ?? new Color(1, 0.54f, 0, 1);
            settings.start.thickness = startThickness;
            settings.start.size = startSize;
            
            settings.end.innerColor = endInnerColor ?? new Color(0.98f, 0.87f, 0.29f, 1);
            settings.end.outerColor = endOuterColor ?? new Color(1, 0.54f, 0, 1);
            settings.end.thickness = endThickness;
            settings.end.size = endSize;            
            
            FlatFx.AddEffect(position, (int) FlatFXType.Ripple);
        }
        
        public Transform CallTutorialTapEffect(Transform parent)
        {
            Transform tap = Instantiate(m_TapVfx, parent.position, Quaternion.identity, parent).transform;
            
            //Make tap independent on parent scale
            tap.localScale = new Vector3(
                tap.localScale.x * tap.localScale.x / tap.lossyScale.x,
                tap.localScale.y * tap.localScale.y / tap.lossyScale.y,
                tap.localScale.z * tap.localScale.z / tap.lossyScale.z);
            
            return tap;
        }
        
        //Editor
#if UNITY_EDITOR
        
        [ContextMenu("Tap")]
        public void EditorCallTapEffect()
        {
            if (Application.IsPlaying(this))
                CallTutorialTapEffect(null);
        }

        [ContextMenu("CallWinningSound")]
        public void EditorCallWinningSound()
        {
            if(Application.IsPlaying(this))
                CallWinningSound();
        }    
        
        [ContextMenu("CallCrosslight")]
        public void EditorCallCrosslightEffect()
        {
            if(Application.IsPlaying(this))
                CallCrosslightEffect(Vector2.zero);
        }
        
        [ContextMenu("CallTapRipple")]
        public void EditorCallTapRippleEffect()
        {
            if(Application.IsPlaying(this))
                CallTapRippleEffect(Vector2.zero);
        }
        
#endif
    }
}