using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class ClocksRotation : MonoBehaviour
{
    [Serializable]
    struct RotationTarget
    {
        [SerializeField] public Transform targetTransform;
        [SerializeField] public float rotationDegree;
        [NonSerialized]  public float defaultRotation;
    } 
    
    [SerializeField] AnimationCurve curve;
    [SerializeField] RotationTarget[] targets;

    void Awake()
    {
        UpdateDefaults();
    }

    void Start()
    {
        StartCoroutine(UpdateRoutine());
    }
    
    IEnumerator UpdateRoutine()
    {
        float duration = curve.keys.Last().time;
        float time = 0;

        while (time < duration)
        {
            yield return null;
            
            time += Time.deltaTime;

            float rotationFactor = curve.Evaluate(time);
            
            foreach (RotationTarget target in targets)
            {
                Vector3 newRotation = target.targetTransform.localRotation.eulerAngles;
                newRotation.z = target.defaultRotation + rotationFactor * target.rotationDegree;
                target.targetTransform.localRotation = Quaternion.Euler(newRotation);
            }
        }
        
        foreach (RotationTarget target in targets)
        {
            Vector3 newRotation = target.targetTransform.localRotation.eulerAngles;
            newRotation.z = target.defaultRotation + target.rotationDegree;
            target.targetTransform.localRotation = Quaternion.Euler(newRotation);
        }
        
        UpdateDefaults();

        StartCoroutine(UpdateRoutine());
    }

    void UpdateDefaults()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            RotationTarget target = targets[i];
            target.defaultRotation = target.targetTransform.localRotation.eulerAngles.z;
            targets[i] = target;
        }
    }
}
