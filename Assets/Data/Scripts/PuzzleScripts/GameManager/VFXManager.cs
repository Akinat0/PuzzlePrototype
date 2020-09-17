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
            int effectNumber = FlatFXType.SunRays.GetHashCode();
            m_LevelCompleteRoutine = StartCoroutine(LevelCompleteSunshineEffectRoutine(position, effectNumber, startState, endState));
        }

        public void StopLevelCompleteSunshineEffect()
        {
            StopCoroutine(m_LevelCompleteRoutine);    
        }

        public void CallCrosslightEffect(Vector2 position)
        {
            FlatFx.useUnscaledTime = false;
            FlatFx.AddEffect(position, FlatFXType.Crosslight.GetHashCode());
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

        public void CallTapRippleEffect(Vector3 position)
        {
            FlatFx.useUnscaledTime = true;
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
                FlatFx.settings[effectNumber].lifetime = 3.0f;
                FlatFx.settings[effectNumber].sectorCount = 20;

                if (start != null && end != null)
                {
                    FlatFx.settings[effectNumber].start.size = start.size;
                    FlatFx.settings[effectNumber].end.size = end.size;

                    FlatFx.settings[effectNumber].start.thickness = start.size;
                    FlatFx.settings[effectNumber].end.thickness = end.size;

                    FlatFx.settings[effectNumber].start.innerColor = start.innerColor;
                    FlatFx.settings[effectNumber].start.outerColor = start.outerColor;

                    FlatFx.settings[effectNumber].end.innerColor = end.innerColor;
                    FlatFx.settings[effectNumber].end.outerColor = end.outerColor;
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