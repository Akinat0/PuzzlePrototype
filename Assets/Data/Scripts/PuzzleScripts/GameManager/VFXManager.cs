using System;
using System.Collections;
using System.Collections.Generic;
using Abu.Tools;
using UnityEngine;

namespace Puzzle
{
    public class VFXManager : MonoBehaviour
    {
        [SerializeField] private Transform[] confettiHolders;
 
        public static VFXManager Instance;
        private FlatFX m_FlatFx;

        private Coroutine LevelCompleteRoutine;
        private GameObject _confettiVfx;

        public FlatFX FlatFx => m_FlatFx;

        private void Awake()
        {
            Instance = this;
            m_FlatFx = GetComponent<FlatFX>();
            _confettiVfx = Resources.Load<GameObject>("Prefabs/Confetti");
            SetConfettiHoldersPositions();
        }
        
        public void CallLevelCompleteSunshineEffect(Vector2 position, FlatFXState startState = null, FlatFXState endState = null)
        {
            int effectNumber = FlatFXType.SunRays.GetHashCode();
            LevelCompleteRoutine = StartCoroutine(LevelCompleteSunshineEffectRoutine(position, effectNumber, startState, endState));
        }

        public void StopLevelCompleteSunshineEffect()
        {
            StopCoroutine(LevelCompleteRoutine);    
        }
        
        IEnumerator LevelCompleteSunshineEffectRoutine(Vector2 position, int effectNumber, FlatFXState start, FlatFXState end)
        {
            while (true)
            {
                FlatFx.settings[effectNumber].lifetime = 5.0f;
                FlatFx.settings[effectNumber].sectorCount = 20;
                
                FlatFx.settings[effectNumber].start.size = start.size;
                FlatFx.settings[effectNumber].end.size = end.size;
                
                FlatFx.settings[effectNumber].start.thickness = start.size;
                FlatFx.settings[effectNumber].end.thickness = end.size;
                
                FlatFx.settings[effectNumber].start.innerColor = start.innerColor;
                FlatFx.settings[effectNumber].start.outerColor = start.outerColor;
                
                FlatFx.settings[effectNumber].end.innerColor = end.innerColor;
                FlatFx.settings[effectNumber].end.outerColor = end.outerColor;
                

                FlatFx.AddEffect(position, effectNumber);
                yield return new WaitForSeconds(2.5f);
            }
        }

        [ContextMenu("Confetti")]
        public void CallConfettiEffect()
        {
            foreach (Transform confettiHolder in confettiHolders)
            {
                if (_confettiVfx != null)
                    Instantiate(_confettiVfx, confettiHolder);
            }
        }

        private void SetConfettiHoldersPositions()
        {
            foreach (Transform confettiHolder in confettiHolders)
                confettiHolder.parent = Camera.main.transform;

            Vector2 camSize = ScreenScaler.CameraSize;

            confettiHolders[0].position = new Vector3(0, -camSize.y/2, 0);
            confettiHolders[1].position = new Vector3(-camSize.x/2, -camSize.y/2, 0);
            confettiHolders[2].position = new Vector3(camSize.x/2, -camSize.y/2, 0);
            confettiHolders[3].position = new Vector3(-camSize.x/2, camSize.y/2, 0);
            confettiHolders[4].position = new Vector3(camSize.x/2, camSize.y/2, 0);
            
            foreach (Transform confettiHolder in confettiHolders)
                confettiHolder.LookAt(Camera.main.transform);
            
        }
    }
}