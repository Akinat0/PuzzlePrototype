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

    [Space(10), SerializeField, Tooltip("Top, Right, Bottom and Left transforms respectively")] 
    private Transform[] TRBL_positions;

    private Animator _animator;
    
    private static readonly int Damaged = Animator.StringToHash("Damaged");

    public Transform[] TRBLPositions => TRBL_positions;

    protected virtual void Start()
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

    public Vector3 GetSidePosition(Side _Side)
    {
        switch (_Side)
        {
            case Side.Up:
                return TRBL_positions[0].position;
            case Side.Right:
                return TRBL_positions[1].position;
            case Side.Down:
                return TRBL_positions[2].position;
            case Side.Left:
                return TRBL_positions[3].position;
            default:
                Debug.LogError("_Side is unreadable");
                return Vector3.one;
        }
    }
    
    public virtual void ChangeSides()
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
