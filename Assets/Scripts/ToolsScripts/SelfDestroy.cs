using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abu.Tools
{
    public class SelfDestroy : MonoBehaviour
    {
        [SerializeField] private float destroyTime = 2;

        void Start()
        {
            Destroy(gameObject, destroyTime);
        }
    }
}