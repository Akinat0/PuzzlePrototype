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
            if (_Phase >= skin.Length || _Phase < 0)
                continue;
            
            skin.Skin = _Phase;
        }
    }
}
