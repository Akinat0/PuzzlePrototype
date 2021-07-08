using System.Collections;
using UnityEngine;

namespace Abu.Tools
{
    public class CoroutineHelper : MonoBehaviour
    {
        static CoroutineHelper helper;

        static CoroutineHelper Helper
        {
            get
            {
                if (helper == null)
                    helper = new GameObject("coroutine_helper").AddComponent<CoroutineHelper>();
                
                return helper;
            }
        }

        public static Coroutine StartRoutine(IEnumerator routine)
        {
            return Helper.StartCoroutine(routine);
        }
        
        public static void StopRoutine(IEnumerator routine)
        {
            Helper.StopCoroutine(routine);
        }
        
        void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}