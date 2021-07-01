using UnityEngine;

namespace Abu.Tools
{
    public class MonoHelper : MonoBehaviour
    {
        void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}