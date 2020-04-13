using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class VFXManager : MonoBehaviour
    {
        private static VFXManager instance;
        private FlatFX m_FlatFx;

        private Coroutine LevelCompleteRoutine;
        
        public static FlatFX FlatFx => instance.m_FlatFx;

        private void Awake()
        {
            m_FlatFx = GetComponent<FlatFX>();
            instance = this;
        }

        public static void CallLevelCompleteEffect(Vector2 position, FlatFXState startState = null, FlatFXState endState = null)
        {
            int effectNumber = FlatFXType.SunRays.GetHashCode();
            instance.LevelCompleteRoutine = instance.StartCoroutine(LevelCompleteEffectRoutine(position, effectNumber, startState, endState));
        }

        public static void StopLevelCompleteEffect()
        {
            instance.StopCoroutine(instance.LevelCompleteRoutine);    
        }
        
        static IEnumerator LevelCompleteEffectRoutine(Vector2 position, int effectNumber, FlatFXState start, FlatFXState end)
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
        
    }
}