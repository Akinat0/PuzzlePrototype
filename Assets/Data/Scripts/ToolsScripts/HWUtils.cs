using UnityEngine;

namespace Abu.Tools
{
    public static class HWUtils
    {

#if UNITY_ANDROID

        public static int ApiVersion
        {
            get
            {
                using (AndroidJavaClass version = new AndroidJavaClass("android.os.Build$VERSION")) {
                    return version.GetStatic<int>("SDK_INT");
                }
            }
        }
#endif
    }
}