using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableView : MonoBehaviour
{
    [SerializeField] private SkinContainer[] _skinContainers;

    public void SetSkinsPhase(int _Phase)
    {
        foreach (SkinContainer skin in _skinContainers)
        {
            if (skin.Length <= _Phase && _Phase < 0)
                continue;
            
            skin.Skin = _Phase;
        }
    }
}
