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

        private Transform[] m_ConfettiHolders = new Transform[5];
        private FlatFX m_FlatFx;
        private Coroutine m_LevelCompleteRoutine;
        private GameObject m_ConfettiVfx;
        private GameObject m_TapVfx;
        private GameObject m_TadaSFX;
        private Volume m_Volume;

        public Vignette Vignette
        {
            get{
                m_Volume.profile.TryGet(out Vignette vignette);
                return vignette;
            }
        }

        public FlatFX FlatFx => m_FlatFx;

        private void Awake()
        {
            Instance = this;
            m_FlatFx = GetComponent<FlatFX>();
            m_Volume = GetComponent<Volume>();
            m_TapVfx = Resources.Load<GameObject>("Prefabs/Tap");
            m_ConfettiVfx = Resources.Load<GameObject>("Prefabs/Confetti");
            m_TadaSFX = Resources.Load<GameObject>("Prefabs/WinningSound");
            SetConfettiHoldersPositions();
        }

        public void CallLevelCompleteSunshineEffect(Vector2 position, FlatFXState startState = null, FlatFXState endState = null)
        {
            int effectNumber = (int)FlatFXType.SunRays;
            
            FlatFx.settings[effectNumber].lifetime = 3.0f;
            FlatFx.settings[effectNumber].sectorCount = 20;

            if (startState != null)
            {
                FlatFx.settings[effectNumber].start.size = startState.size;
                FlatFx.settings[effectNumber].start.thickness = startState.size;
                FlatFx.settings[effectNumber].start.innerColor = startState.innerColor;
                FlatFx.settings[effectNumber].start.outerColor = startState.outerColor;
            }

            if (endState != null)
            {
                FlatFx.settings[effectNumber].end.size = endState.size;
                FlatFx.settings[effectNumber].end.thickness = endState.size;
                FlatFx.settings[effectNumber].end.innerColor = endState.innerColor;
                FlatFx.settings[effectNumber].end.outerColor = endState.outerColor;
            }
            
            FlatFx.AddEffect(position, effectNumber);
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
        
        public void CallConfettiEffect()
        {
            foreach (Transform confettiHolder in m_ConfettiHolders)
            {
                if (m_ConfettiVfx != null)
                    Instantiate(m_ConfettiVfx, confettiHolder);
            }
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
            
            FlatFx.AddEffect(position, FlatFXType.Ripple.GetHashCode());
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
        
        public Transform CallTutorialTapEffect(Transform parent, Color color)
        {
            Transform tap = CallTutorialTapEffect(parent);

            foreach (SpriteRenderer spriteRenderer in tap.GetComponentsInChildren<SpriteRenderer>())
                spriteRenderer.color = color;

            return tap;
        }

        private void SetConfettiHoldersPositions()
        {
            for(int i = 0; i < 5; i++)
                m_ConfettiHolders[i] = new GameObject("ConfettiHolder_" + i).transform;
            
            foreach (Transform confettiHolder in m_ConfettiHolders)
                confettiHolder.parent = Camera.main.transform;

            Vector2 camSize = ScreenScaler.CameraSize;

            m_ConfettiHolders[0].position = new Vector3(0, -camSize.y/2, 3);
            m_ConfettiHolders[0].gameObject.SetActive(false);
            m_ConfettiHolders[1].position = new Vector3(-camSize.x/2, -camSize.y/2, 3);
            m_ConfettiHolders[2].position = new Vector3(camSize.x/2, -camSize.y/2, 3);
            m_ConfettiHolders[3].position = new Vector3(-camSize.x/2, camSize.y/2, 3);
            m_ConfettiHolders[4].position = new Vector3(camSize.x/2, camSize.y/2, 3);
            
            foreach (Transform confettiHolder in m_ConfettiHolders)
                confettiHolder.LookAt(new Vector3(0, 0, 0));
            
        }
        
        IEnumerator LevelCompleteSunshineEffectRoutine(Vector2 position, int effectNumber, FlatFXState start, FlatFXState end)
        {
            while (true)
            {

                if (start != null && end != null)
                {
                    
                }


                FlatFx.AddEffect(position, effectNumber);
                yield return new WaitForSeconds(2.5f);
            }
        } 
        
        //Editor
#if UNITY_EDITOR
        
        [ContextMenu("Tap")]
        public void EditorCallTapEffect()
        {
            if (Application.IsPlaying(this))
                CallTutorialTapEffect(null);
        }
        
        [ContextMenu("Confetti")]
        public void EditorCallConfettiEffect()
        {
            if (Application.IsPlaying(this))
                CallConfettiEffect();
        }

        [ContextMenu("CompleteSunshineStart")]
        public void EditorCallSunshineStart()
        {
            if(Application.IsPlaying(this))
                CallLevelCompleteSunshineEffect(Vector2.zero);
        }

        [ContextMenu("CompleteSunshineStop")]
        public void EditorEndCompleteSunshine()
        {
            if(Application.IsPlaying(this))
                StopLevelCompleteSunshineEffect();
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