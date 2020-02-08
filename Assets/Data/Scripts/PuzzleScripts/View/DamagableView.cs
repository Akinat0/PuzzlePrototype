using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableView : MonoBehaviour
{
    [SerializeField] private SkinContainer[] _skinContainers;

    public bool SetSkinsPhase(int _Phase)
    {
        foreach (SkinContainer skin in _skinContainers)
        {
            //TODO merge skin container refactoring
        }

        return false;
    }
}
