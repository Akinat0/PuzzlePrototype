using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonPlayerView : PlayerView
{
    private SkinContainer _shapeSkinContainer;
    
    protected override void Start()
    {
        base.Start();
        _shapeSkinContainer = shape.GetComponent<SkinContainer>();
        _shapeSkinContainer.Skin = 0;
    }

    public override void ChangeSides()
    {
        _shapeSkinContainer.Skin = _shapeSkinContainer.Skin == 0 ? 1 : 0;
    }
}
