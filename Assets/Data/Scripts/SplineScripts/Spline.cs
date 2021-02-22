using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Spline : MonoBehaviour, IEnumerable<Spline.Point>
{
    #region nested

    [Serializable]
    public struct Anchor
    {
        [SerializeField] Vector3 position;
        [SerializeField] Vector3 inTangent;
        [SerializeField] Vector3 outTangent;

        //tangents are relative to position
        public Vector3 Position
        {
            get => position;
            set => position = value;
        }

        public Vector3 InTangent
        {
            get => inTangent;
            set => inTangent = value;
        }

        public Vector3 OutTangent
        {
            get => outTangent;
            set => outTangent = value;
        }

        public Anchor(Vector3 position, Vector3 inTangent, Vector3 outTangent)
        {
            this.position = position;
            this.inTangent = inTangent;
            this.outTangent = outTangent;
        }
    }
    
    public struct Point
    {
        public Vector3 Position { get; private set; }
        
        public float Phase { get; private set; }
        
        public Point(Vector3 position, float phase)
        {
            Position = position;
            Phase = phase;
        }
    }
    
    #endregion
    
    #region public attributes

    public event Action OnRebuild;
    
    public Point this[int index] => Points[index];

    public int Length => Points.Count;
    
    #endregion
    
    #region serialized

    [SerializeField, HideInInspector] 
    List<Anchor> anchors;
    
    [SerializeField] int samples = 10;
    [SerializeField] bool looped;
    [SerializeField] bool uniform;

    public int Samples
    {
        get => samples;
        set
        {
            if(samples == value)
                return;

            samples = value;
            isDirty = true;
        }
    }
    
    public bool Looped
    {
        get => looped;
        set
        {
            if(looped == value)
                return;

            looped = value;
            isDirty = true;
        }
    }
    
    public bool Uniform
    {
        get => uniform;
        set
        {
            if(uniform == value)
                return;

            uniform = value;
            isDirty = true;
        }
    }

    public List<Anchor> Anchors
    {
        get
        {
            if (anchors == null)
                anchors = new List<Anchor>();

            return anchors;
        }
    }
    
    #endregion

    #region private attributes

    bool isDirty;
    
    readonly List<float> LUT = new List<float>();
    readonly List<Point> Points = new List<Point>();

    #endregion

    #region public
    public void Rebuild()
    {
        GenerateLUT();
		
        if (Looped)
            GenerateLoopPoints();
        else
            GenerateStraightPoints();

        OnRebuild?.Invoke();
    }

    public int GetAnchorsCount()
    {
        return Anchors.Count;
    }

    public Anchor[] GetAnchors()
    {
        return Anchors.ToArray();
    }
    
    public Anchor GetAnchor(int index)
    {
        return Anchors[index];
    }
    
    public void RemoveAnchorAt(int index)
    {
        Anchors.RemoveAt(index);
        isDirty = true;
    }
    
    public void AddAnchor(Anchor anchor)
    {
        Anchors.Add(anchor);
        isDirty = true;
    }
    
    public void SetAnchor(Anchor anchor, int index)
    {
        Anchors[index] = anchor;
        isDirty = true;
    }
    
    #endregion

    #region private
    
    void GenerateLUT()
    {
        LUT.Clear();
		
        if (Anchors.Count < 2)
            return;
		
        int samplesCount = Mathf.Max(Looped ? 2 : 1, Samples);
		
        float step   = 1.0f / samplesCount;
        float length = 0;
        
        for (int i = 0; i <= samplesCount; i++)
        {
            float sourcePhase = step * Mathf.Max(0, i - 1);
            float targetPhase = step * i;
			
            Vector3 sourcePosition = GetPosition(sourcePhase);
            Vector3 targetPosition = GetPosition(targetPhase);
			
            length += Vector3.Distance(targetPosition, sourcePosition);
			
            LUT.Add(length);
        }
    }

    Vector3 GetPosition(float phase)
    {
        if (Anchors == null || Anchors.Count == 0)
            return Vector3.zero;

        float step;
        int index;

        if (Looped)
        {
            step = 1.0f / Anchors.Count;
            index = Mathf.Min(Mathf.FloorToInt(phase * Anchors.Count), Anchors.Count - 1);
        }
        else
        {
            step = 1.0f / (Anchors.Count - 1);
            index = Mathf.FloorToInt(phase * (Anchors.Count - 1));
        }

        Anchor source = Anchors[index];
        Anchor target = Anchors[(index + 1) % Anchors.Count];

        float resultPhase = step * index;

        return CubicInterpolation(
            source.Position,
            source.Position + source.OutTangent,
            target.Position + target.InTangent,
            target.Position,
            Mathf.InverseLerp(resultPhase, resultPhase + step, phase)
        );
    }
    
    Vector3 GetUniformPosition(float phase)
    {
        if (LUT == null || LUT.Count == 0)
            return GetPosition(phase);
        
        float length = phase * LUT[LUT.Count - 1];
		
        int i = 0;
        int j = LUT.Count - 1;
        if (LUT.Count > 2)
        {
            while (i < j)
            {
                int k = i + (j - i) / 2;
				
                float value = LUT[k];
				
                if (Mathf.Approximately(value, length))
                {
                    i = j = k;
                    break;
                }
				
                if (value > length)
                    j = k;
                else
                    i = k;
				
                if (j - i <= 1)
                    break;
            }
        }
		
        float step        = 1.0f / (LUT.Count - 1);
        float sourcePhase = step * i;
        float targetPhase = step * j;
		
        float sourceLength = LUT[i];
        float targetLength = LUT[j];
		
        float resultPhase = Mathf.Lerp(
            sourcePhase,
            targetPhase,
            Mathf.InverseLerp(sourceLength, targetLength, length)
        );
		
        return GetPosition(resultPhase);
    }
    
    void GenerateStraightPoints()
    {
        Points.Clear();
        
        if (Anchors == null || Anchors.Count < 2)
            return;
		
        List<Vector3> positions = new List<Vector3>();

        samples = Mathf.Max(1, Samples);
        
        float step = 1.0f / Samples;
        
        for (int i = 0; i <= Samples; i++)
        {
            Vector3 position = Uniform
                ? GetUniformPosition(step * i)
                : GetPosition(step * i);
			
            positions.Add(position);
        }
		
        for (int i = 0; i < positions.Count; i++)
        {
            Vector3 position = positions[i];
            float   phase    = 1.0f / (positions.Count - 1) * i;
			
            Points.Add(new Point(position, phase));
        }
    }
    
    void GenerateLoopPoints()
    {
        Points.Clear();

        if (Anchors.Count < 2)
            return;
        
        List<Vector3> positions = new List<Vector3>();
		
        samples = Mathf.Max(2, Samples);
        
        float step    = 1.0f / Samples;
        for (int i = 0; i < Samples; i++)
        {
            Vector3 position = Uniform
                ? GetUniformPosition(step * i)
                : GetPosition(step * i);
			
            positions.Add(position);
        }
        
        for (int i = 0; i < positions.Count; i++)
        {
            Vector3 position = positions[i];
            float   phase    = 1.0f / positions.Count * i;
			
            Points.Add(new Point(position, phase));
        }
    }
    
    static Vector3 CubicInterpolation(Vector3 sourcePosition, Vector3 sourceTangent, Vector3 targetPosition, Vector3 targetTangent, float phase)
    {
        float p1 = Mathf.Clamp01(phase);
        float p2 = p1 * p1;
        float p3 = p2 * p1;
		
        float v1 = 1 - p1;
        float v2 = v1 * v1;
        float v3 = v2 * v1;
		
        return v3 * sourcePosition + v2 * p1 * sourceTangent * 3 + v1 * p2 * targetPosition * 3 + p3 * targetTangent;
    }

    #endregion
    
    #region engine

    void OnEnable()
    {
        Rebuild();
    }

    void LateUpdate()
    {
        if (isDirty)
        {
            isDirty = false;
			
            Rebuild();
        }
    }

    void OnValidate()
    {
        Rebuild();
    }

    void OnDidApplyAnimationProperties()
    {
        Rebuild();
    }

    #endregion
    
    #region IEnumerable

    public IEnumerator<Point> GetEnumerator()
    {
        return Points.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    #endregion
}
