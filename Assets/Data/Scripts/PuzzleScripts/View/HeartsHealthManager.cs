﻿using Puzzle;
using UnityEngine;

public class HeartsHealthManager : HealthManagerBase
{

    [SerializeField] private Animator HeartsAnimator;
    [SerializeField] private HeartView[] hearts;
    
    private static readonly int AnimationReset = Animator.StringToHash("Reset");

    private void Start()
    { 
        Hp = hearts.Length - 1;
    }

    protected override void LoseHeart(int _Hp)
    {
        if (Hp < 0)
            return;

        HeartView heart = hearts[Hp];
        
        if(heart!=null)
            heart.Disappear();
        else
            Debug.LogError($"Heart {Hp} is null");
        
        Hp--;
        
        if(_Hp != Hp)
            Debug.LogWarning("Health manager hp doesn't match with player's hp");

    }

    protected override void Reset()
    {
        if(HeartsAnimator != null)
            HeartsAnimator.SetTrigger(AnimationReset);
        
        Hp = hearts.Length - 1;
        foreach (var heart in hearts)
            heart.Appear();
    }
}