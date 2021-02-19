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

        SplineEditorUtility.DrawSpline(Spline);
        DrawSceneNormals();
    }

    void DrawSceneNormals()
    {
        Matrix4x4 matrix = Handles.matrix;
        Handles.matrix = Spline.transform.localToWorldMatrix;
        
        Vector3[] normals = Spline.GetNormals2D();

        Color handlesColor = Handles.color;
        Handles.color = Color.red;

        for (int i = 0; i < Spline.Length; i++)
        {
            Handles.DrawSolidDisc(Spline[i].Position, Follower2D.transform.forward, SplineEditorUtility.HandlesSize * HandleUtility.GetHandleSize(Spline[i].Position));
            Handles.DrawLine(Spline[i].Position, Spline[i].Position + normals[i]);
        }

        Handles.color = handlesColor;
        
        Handles.matrix = matrix;
    }
}
