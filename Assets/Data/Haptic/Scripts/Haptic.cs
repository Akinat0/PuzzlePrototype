public class Haptic
{
    #region nested types

    public enum Type
    {
        NONE          = -1,
        DEFAULT       = 0,
        SELECTION     = 1,
        SUCCESS       = 2,
        WARNING       = 3,
        FAILURE       = 4,
        IMPACT_LIGHT  = 5,
        IMPACT_MEDIUM = 6,
        IMPACT_HEAVY  = 7
    }

    #endregion

    #region construction

    static Haptic instance;
    
    static Haptic Instance
    {
        get
        {
            if (instance == null)
            {
#if UNITY_EDITOR
                instance = new Haptic();
#elif UNITY_ANDROID
			    instance = new HapticAndroid();
#elif UNITY_IOS
			    instance = new HapticIOs();
#else
			    instance = new Haptic();
#endif
            }

            return instance;
        }
    }

    #endregion

    #region public methods

    public static void Run(Haptic.Type _Type)
    {
        if (_Type > Type.NONE)
            Instance.RunInternal(_Type);
    }

    #endregion

    #region service methods

    protected virtual void RunInternal(Haptic.Type _Type)
    {
        // Do nothing
    }
    
    #endregion
}