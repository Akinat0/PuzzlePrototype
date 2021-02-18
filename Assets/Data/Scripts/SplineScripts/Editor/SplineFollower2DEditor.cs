using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SplineFollower2D))]
public class SplineFollower2DEditor : Editor
{
    Spline Spline => Follower2D.Spline;
    SplineFollower2D Follower2D => target as SplineFollower2D;
    void OnSceneGUI()
    {
        if(Spline == null)
            return;
        
        Matrix4x4 matrix = Handles.matrix;
        Handles.matrix = Spline.transform.localToWorldMatrix;
        
        DrawSceneSpline();
        DrawSceneNormals();
        
        Handles.matrix = matrix;
    }

    void DrawSceneSpline()
    {
        int length = Spline.Looped
            ? Spline.GetAnchorsCount()
            : Spline.GetAnchorsCount() - 1;
        
        for (int i = 0; i < length; i++)
        {
            Spline.Anchor sourceAnchor = Spline.GetAnchor(i);
            Spline.Anchor targetAnchor = Spline.GetAnchor((i + 1) % Spline.GetAnchorsCount());
			
            Handles.DrawBezier(
                sourceAnchor.Position,
                targetAnchor.Position,
                sourceAnchor.Position + sourceAnchor.OutTangent,
                targetAnchor.Position + targetAnchor.InTangent,
                Color.white,
                null,
                2
            );
        }
    }

    void DrawSceneNormals()
    {
        Vector3[] normals = Spline.GetNormals2D();

        Color handlesColor = Handles.color;
        Handles.color = Color.red;

        for (int i = 0; i < Spline.Length; i++)
        {
            Handles.DrawSolidDisc(Spline[i].Position, Follower2D.transform.forward, 1);
            Handles.DrawLine(Spline[i].Position, Spline[i].Position + normals[i] * 10);
        }

        Handles.color = handlesColor;
    }
}
