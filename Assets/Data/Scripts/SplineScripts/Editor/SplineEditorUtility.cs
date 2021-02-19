using UnityEditor;
using UnityEngine;

public static class SplineEditorUtility
{
    const string SplineHandlesSizeKey = "spline_handles_size";

    static float handlesSize = -1;
    public static float HandlesSize => handlesSize > 0 ? handlesSize : EditorPrefs.GetFloat(SplineHandlesSizeKey, 0.1f);

    public static void DrawSplineAnchors(Spline spline, Color anchorsColor)
    {
        Matrix4x4 matrix = Handles.matrix;
        Handles.matrix = spline.transform.localToWorldMatrix;
        
        Spline.Anchor[] anchors = spline.GetAnchors();
        Color handlesColor = Handles.color;
        
        Vector3 camPosition = spline.transform.InverseTransformPoint(SceneView.currentDrawingSceneView.camera.transform.position);
        
        Handles.color = anchorsColor;
        
        foreach (Spline.Anchor anchor in anchors)
        {
            Handles.DrawSolidDisc(anchor.Position, (anchor.Position - camPosition).normalized, HandlesSize * HandleUtility.GetHandleSize(anchor.Position));
            Vector3 inTangentPosition = anchor.Position + anchor.InTangent;
            Vector3 outTangentPosition = anchor.Position + anchor.OutTangent;
            
            Handles.DrawLine(anchor.Position, inTangentPosition);
            Handles.DrawLine(anchor.Position, outTangentPosition);
            
            Handles.DrawSolidDisc(inTangentPosition, (inTangentPosition - camPosition).normalized, HandlesSize * HandleUtility.GetHandleSize(inTangentPosition));
            Handles.DrawSolidDisc(outTangentPosition, (outTangentPosition - camPosition).normalized, HandlesSize * HandleUtility.GetHandleSize(outTangentPosition));
        }

        Handles.color = handlesColor;
        
        Handles.matrix = matrix;
    }

    public static void DrawSplinePoints(Spline spline)
    {
        Matrix4x4 matrix = Handles.matrix;
        Handles.matrix = spline.transform.localToWorldMatrix;
        
        Vector3 camPosition = spline.transform.InverseTransformPoint(SceneView.currentDrawingSceneView.camera.transform.position);
            
        foreach (Spline.Point point in spline)
            Handles.DrawSolidDisc(point.Position, (camPosition - point.Position).normalized, HandlesSize * HandleUtility.GetHandleSize(point.Position));

        Handles.matrix = matrix;
    }

    public static void DrawSpline(Spline spline)
    {
        Matrix4x4 matrix = Handles.matrix;
        Handles.matrix = spline.transform.localToWorldMatrix;
        
        int length = spline.Looped
            ? spline.GetAnchorsCount()
            : spline.GetAnchorsCount() - 1;
        
        for (int i = 0; i < length; i++)
        {
            Spline.Anchor sourceAnchor = spline.GetAnchor(i);
            Spline.Anchor targetAnchor = spline.GetAnchor((i + 1) % spline.GetAnchorsCount());
			
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
        
        Handles.matrix = matrix;
    }

    public static void DrawSplinePointsSizeField()
    {
        EditorGUI.BeginChangeCheck();
        
        handlesSize = EditorGUILayout.Slider("Points size: ",HandlesSize, 0.01f, 0.1f);

        if (EditorGUI.EndChangeCheck())
        {
            EditorPrefs.SetFloat(SplineHandlesSizeKey, handlesSize);
            SceneView.RepaintAll();
        }
    }
}
