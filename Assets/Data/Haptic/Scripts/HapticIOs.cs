#if UNITY_IOS && !UNITY_EDITOR

using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.iOS;

class HapticIOs : Haptic
{
    #region imports

    [DllImport("__Internal")]
    static extern void InitializeHapticGenerators();

    [DllImport("__Internal")]
    static extern void HapticSelection();

    [DllImport("__Internal")]
    static extern void HapticSuccess();

    [DllImport("__Internal")]
    static extern void HapticWarning();

    [DllImport("__Internal")]
    static extern void HapticFailure();

    [DllImport("__Internal")]
    static extern void HapticLightImpact();

    [DllImport("__Internal")]
    static extern void HapticMediumImpact();

    [DllImport("__Internal")]
    static extern void HapticHeavyImpact();

    #endregion

    #region properties

    bool SupportsHaptic => Device.generation >= DeviceGeneration.iPhone7;

    #endregion

    #region constructor

    public HapticIOs()
    {
        try
        {
            if(SupportsHaptic)
                InitializeHapticGenerators();
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to initialize haptic : " + e.Message);
        }
    }

    #endregion

    #region service methods

    protected override void RunInternal(Haptic.Type type)
    {
        if (type == Type.DEFAULT)
        {
            Handheld.Vibrate();
            return;
        }

        if (!SupportsHaptic)
            return;
        
        try
        {
            switch (type)
            {
                case Type.SELECTION:
                    HapticSelection();
                    return;
                case Type.SUCCESS:
                    HapticSuccess();
                    return;
                case Type.WARNING:
                    HapticWarning();
                    return;
                case Type.FAILURE:
                    HapticFailure();
                    return;
                case Type.IMPACT_LIGHT:
                    HapticLightImpact();
                    return;
                case Type.IMPACT_MEDIUM:
                    HapticMediumImpact();
                    return;
                case Type.IMPACT_HEAVY:
                    HapticHeavyImpact();
                    return;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to run haptic " + type + " : " + e.Message);
        }
    }

    #endregion
}

#endif