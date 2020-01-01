using System;
using System.Collections;
using System.Collections.Generic;
using Abu.Tools;
using Puzzle;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    
    [SerializeField] private float _partOfScreen = 0.25f;
    [SerializeField] public Transform shape;

    private Animator _animator;
    
    private static readonly int Damaged = Animator.StringToHash("Damaged");

    void Start()
    {
        _animator = GetComponent<Animator>();
        SetScale(_partOfScreen);
    }
    
    void SetScale(float partOfScreen)
    {
        transform.localScale = Vector3.one * 
            ScreenScaler.ScaleToFillPartOfScreen(
                shape.gameObject.GetComponent<SpriteRenderer>(),
                partOfScreen);
    }

    public void ChangeSides()
    {
        shape.Rotate(0, 0, 180);
    }

    private void OnEnable()
    {
        GameSceneManager.PlayerLosedHpEvent += PlayerLosedHpEvent_Handler;
    }

    private void OnDisable()
    {
        GameSceneManager.PlayerLosedHpEvent -= PlayerLosedHpEvent_Handler;
    }

    void PlayerLosedHpEvent_Handler(int hp)
    {
        _animator.SetTrigger(Damaged);
    }
}
