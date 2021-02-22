using UnityEngine;

[ExecuteInEditMode]
public class SplineFollower2D : MonoBehaviour
{
    [SerializeField] Spline spline;
    [SerializeField, Range(0, 1)] float phase;

    Transform cachedTransform;
    Transform Transform => cachedTransform != null ? cachedTransform : cachedTransform = transform;

    public Spline Spline
    {
        get => spline;
        set
        {
            if (spline == value)
                return;

            spline.OnRebuild -= UpdateNormals;
            spline = value;
            spline.OnRebuild += UpdateNormals;
			
            UpdateNormals();
        }
    }

    Vector3[] normals;

    Vector3[] Normals => normals ?? (normals = Spline.GetNormals2D());

    public float Phase
    {
        get => phase;
        set
        {
            if (Mathf.Approximately(phase, value))
                return;
			
            phase = value;
			
            ProcessPhase();
        }
    }

    void OnDidApplyAnimationProperties()
    {
        ProcessPhase();
    }

    void OnEnable()
    {
        if(Spline == null)
            return;

        Spline.OnRebuild += UpdateNormals;
        
        UpdateNormals();
    }
    
    void OnDisable()
    {
        if(Spline == null)
            return;

        Spline.OnRebuild -= UpdateNormals;
    }

    void OnValidate()
    {
        Spline.Rebuild();
        UpdateNormals();
    }

    void UpdateNormals()
    {
        if(Spline == null)
            return;
        normals = Spline.GetNormals2D();
        ProcessPhase();
    }
    
    void ProcessPhase()
    {
        if(Spline == null)
            return;
        
        if (Spline.Looped)
            ProcessLoopPhase();
        else
            ProcessStraightPhase();
    }

    void ProcessLoopPhase()
    {
        if(Spline.Length == 0)
            return;
        
        Phase = Mathf.Repeat(Phase, 1);

        float value = Phase * Spline.Length;
        float step = 1.0f / Spline.Length;

        int sourceIndex = Mathf.Min(Mathf.FloorToInt(value), Spline.Length - 1);
        int targetIndex = Mathf.Min(sourceIndex + 1, Spline.Length) % Spline.Length;

        Spline.Point source = Spline[sourceIndex];
        Spline.Point target = Spline[targetIndex];

        float sourcePhase = step * sourceIndex;
        float targetPhase = sourcePhase + step;
        
        Vector3 sourceNormal = Normals[sourceIndex];
        Vector3 targetNormal = Normals[targetIndex];

        Vector3 position = Vector3.Lerp(
            source.Position,
            target.Position,
            Mathf.InverseLerp(sourcePhase, targetPhase, phase)
        );
        
        Vector3 normal = Vector3.Lerp(
            sourceNormal,
            targetNormal,
            Mathf.InverseLerp(sourcePhase, targetPhase, phase)
        );

        Transform.position = Spline.transform.TransformPoint(position);
        Transform.right = normal;
    }

    void ProcessStraightPhase()
    {
        if(Spline.Length == 0)
            return;
        
        float step = 1.0f / (Spline.Length - 1);
        float value = Phase / step;

        int sourceIndex = Mathf.FloorToInt(value);
        int targetIndex = Mathf.Min(sourceIndex + 1, Spline.Length - 1);

        Spline.Point source = Spline[sourceIndex];
        Spline.Point target = Spline[targetIndex];

        float sourcePhase = step * sourceIndex;
        float targetPhase = step * targetIndex;

        Vector3 sourceNormal = Normals[sourceIndex];
        Vector3 targetNormal = Normals[targetIndex];
        
        Vector3 position = Vector3.Lerp(
            source.Position,
            target.Position,
            Mathf.InverseLerp(sourcePhase, targetPhase, Phase)
        );
        
        Vector3 normal = Vector3.Lerp(
            sourceNormal,
            targetNormal,
            Mathf.InverseLerp(sourcePhase, targetPhase, phase)
        );

        Transform.position = Spline.transform.TransformPoint(position);
        Transform.right = Spline.transform.TransformVector(normal);
    }
}