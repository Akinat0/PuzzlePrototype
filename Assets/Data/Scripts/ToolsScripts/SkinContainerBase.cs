using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class SkinContainerBase : MonoBehaviour
{
    [SerializeField] protected Sprite[] _Sprites;
    protected SpriteRenderer _SpriteRenderer;
    protected int index;
    
    protected virtual void Start()
    {
        _SpriteRenderer = GetComponent<SpriteRenderer>();
    }
}
