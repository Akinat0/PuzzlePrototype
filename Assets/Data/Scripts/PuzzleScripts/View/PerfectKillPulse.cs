using System;
using UnityEngine;

namespace Puzzle
{
    [RequireComponent(typeof(Animator))]
    public class PerfectKillPulse : MonoBehaviour
    {
        static readonly int Play = Animator.StringToHash("Play");
        
        Animator Animator { get; set; }
        void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        void OnEnable()
        {
            GameSceneManager.PerfectKillEvent += PerfectKillEvent_Handler;
        }
        
        void OnDisable()
        {
            GameSceneManager.PerfectKillEvent -= PerfectKillEvent_Handler;
        }

        void PerfectKillEvent_Handler()
        {
            Animator.SetTrigger(Play);
        }
    }
}