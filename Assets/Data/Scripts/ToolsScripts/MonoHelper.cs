using System.Collections;
using System.Collections.Generic;
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