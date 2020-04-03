using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInput : MobileGameInput
{
    
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnEnable()
    {
        Condition = true;
        base.OnEnable();
    }
}
