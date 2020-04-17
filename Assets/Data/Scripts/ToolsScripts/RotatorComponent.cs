using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Abu.Tools
{
    public class RotatorComponent : MonoBehaviour
    {
        [SerializeField] private Vector3 rotationFactor;
        
        [SerializeField, Header("Random")]
        private bool isRandom;
        
        [SerializeField, Tooltip("Only if random enabled")]
        private Vector3 randomDelta;
        
        [SerializeField]
        private bool randomSign = true;


        private Vector3 rotation;

        private void Start()
        {
            if (isRandom)
            {
                for (int i = 0; i < 3; i++)
                {
                    float max = rotationFactor[i] + randomDelta[i];
                    float min = rotationFactor[i] - randomDelta[i];
                    
                    rotation[i] = Random.Range(min, max) * (randomSign ? Mathf.Sign(Random.Range(-1, 1)) : 1);
                }
            }
            else
            {
                rotation = rotationFactor;
            }
        }

        void Update()
        {
            transform.Rotate(rotation * Time.deltaTime);
        }
    }
}

