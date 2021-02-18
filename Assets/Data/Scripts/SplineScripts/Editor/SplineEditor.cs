using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(Spline))]
public class SplineEditor : Editor
{
    Spline Spline => target as Spline;

    bool isEditMode;
    int selectedAnchor = -1;

    ReorderableList anchorsList;
    
    #region inspector

    void OnEnable()
    {
        CreateAnchorsList();
    }

    void CreateAnchorsList()
    {
        anchorsList = new ReorderableList(serializedObject, serializedObject.FindProperty("anchors"), true, true, true, true);
        anchorsList.elementHeight = 130;
        
        anchorsList.drawHeaderCallback += rect =>
        {
            EditorGUI.LabelField(rect, "Anchors", EditorStyles.boldLabel);
        };
        
        anchorsList.drawElementCallback += (rect, index, active, focused) =>
        {
            SerializedProperty keyProperty        = anchorsList.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty positionProperty   = keyProperty.FindPropertyRelative("position");
            SerializedProperty inTangentProperty  = keyProperty.FindPropertyRelative("inTangent");
            SerializedProperty outTangentProperty = keyProperty.FindPropertyRelative("outTangent");
			
            float step = rect.height / 3;
			
            Rect positionRect = new Rect(
                rect.x,
                rect.y + step * 0,
                rect.width,
                step
            );
			
            Rect inTangentRect = new Rect(
                rect.x,
                rect.y + step * 1,
                rect.width,
                step
            );
			
            Rect outTangentRect = new Rect(
                rect.x,
                rect.y + step * 2,
                rect.width,
                step
            );
			
            EditorGUI.BeginChangeCheck();
            
            positionProperty.vector3Value   = EditorGUI.Vector3Field(positionRect, "Position", positionProperty.vector3Value);
            inTangentProperty.vector3Value  = EditorGUI.Vector3Field(inTangentRect, "In", inTangentProperty.vector3Value);
            outTangentProperty.vector3Value = EditorGUI.Vector3Field(outTangentRect, "Out", outTangentProperty.vector3Value);

            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        };
        
        
        anchorsList.drawElementBackgroundCallback += (rect, index, active, focused) =>
        {
            if(active || focused)
                EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f));
            else
                EditorGUI.DrawRect(rect,index % 2 == 0 ? new Color(0.25f, 0.25f, 0.25f) : new Color(0.3f, 0.3f, 0.3f));
        };

        
        anchorsList.onSelectCallback += list =>
        {
            selectedAnchor = list.index;
            SceneView.RepaintAll();
        };

        anchorsList.onRemoveCallback += list =>
        {
            if(selectedAnchor < 0)
                return;

            Spline.RemoveAnchorAt(selectedAnchor);
            selectedAnchor = -1;
          
            Spline.Rebuild();
            SceneView.RepaintAll();
        };

        anchorsList.onAddCallback += list =>
        {
            Spline.AddAnchor(Spline.Length > 0
                ? Spline.GetAnchor(selectedAnchor < 0 ? selectedAnchor : Spline.GetAnchorsCount() - 1)
                : new Spline.Anchor(Vector3.zero, Vector3.one, -Vector3.one));

            selectedAnchor = Spline.GetAnchorsCount() - 1;
            anchorsList.index = selectedAnchor;
            
            Spline.Rebuild();
            SceneView.RepaintAll();
        };
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        DrawConvertTo2D();
        DrawEditModeButton();
        DrawAnchorsList();
    }

    void DrawAnchorsList()
    {
        GUILayout.Space(20);
        anchorsList.DoLayoutList();
    }
    
    void DrawEditModeButton()
    {
        if (GUILayout.Button(isEditMode ? "DISABLE EDIT MODE" : "ENABLE EDIT MODE"))
        {
            isEditMode = !isEditMode;
            SceneView.RepaintAll();
        }
    }
    
    void DrawConvertTo2D()
    {
        if (GUILayout.Button("CONVERT TO 2D"))
        {
            for (int i = 0; i < Spline.GetAnchorsCount(); i++)
            {
                Spline.Anchor anchor = Spline.GetAnchor(i);
                
                anchor.Position = new Vector3(anchor.Position.x, anchor.Position.y, 0);
                anchor.InTangent = new Vector3(anchor.InTangent.x, anchor.InTangent.y, 0);
                anchor.OutTangent = new Vector3(anchor.OutTangent.x, anchor.OutTangent.y, 0);
                
                Spline.SetAnchor(anchor, i);
            }
            
            Spline.Rebuild();
            SceneView.RepaintAll();
        }
    }
    
    #endregion

    #region scene
    
    void OnSceneGUI()
    {
        Matrix4x4 matrix = Handles.matrix;
        Handles.matrix = Spline.transform.localToWorldMatrix;
        
        DrawSceneSpline();
        DrawScenePoints();
        DrawSceneAnchors();
        DrawAnchorsHandlers();
        DrawSelectedHandler();
        
        Handles.matrix = matrix;
    }
    
    void DrawSceneSpline()
    {
        SerializedProperty keysProperty = serializedObject.FindProperty("anchors");
        SerializedProperty loopProperty = serializedObject.FindProperty("looped");
		
        int length = loopProperty.boolValue
            ? keysProperty.arraySize
            : keysProperty.arraySize - 1;
        
        for (int i = 0; i < length; i++)
        {
            SerializedProperty sourceKeyProperty = keysProperty.GetArrayElementAtIndex(i);
            SerializedProperty targetKeyProperty = keysProperty.GetArrayElementAtIndex((i + 1) % keysProperty.arraySize);
			
            SerializedProperty sourcePositionProperty = sourceKeyProperty.FindPropertyRelative("position");
            SerializedProperty sourceTangentProperty  = sourceKeyProperty.FindPropertyRelative("outTangent");
			
            SerializedProperty targetPositionProperty = targetKeyProperty.FindPropertyRelative("position");
            SerializedProperty targetTangentProperty  = targetKeyProperty.FindPropertyRelative("inTangent");
			
            Handles.DrawBezier(
                sourcePositionProperty.vector3Value,
                targetPositionProperty.vector3Value,
                sourcePositionProperty.vector3Value + sourceTangentProperty.vector3Value,
                targetPositionProperty.vector3Value + targetTangentProperty.vector3Value,
                Color.white,
                null,
                2
            );
        }
    }
    
    void DrawScenePoints()
    {
        Vector3 camPosition = SceneView.currentDrawingSceneView.camera.transform.position;
            
        foreach (Spline.Point point in Spline)
        {
            Handles.DrawSolidDisc(point.Position, (point.Position - camPosition).normalized, 1);
        }
    }

    void DrawSceneAnchors()
    {
        Spline.Anchor[] anchors = Spline.GetAnchors();
        Color handlesColor = Handles.color;
        
        Vector3 camPosition = SceneView.currentDrawingSceneView.camera.transform.position;
        
        Handles.color = Color.yellow;
        
        foreach (Spline.Anchor anchor in anchors)
        {
            Handles.DrawSolidDisc(anchor.Position, (anchor.Position - camPosition).normalized, 1);
            Vector3 inTangentPosition = anchor.Position + anchor.InTangent;
            Vector3 outTangentPosition = anchor.Position + anchor.OutTangent;
            
            Handles.DrawLine(anchor.Position, inTangentPosition);
            Handles.DrawLine(anchor.Position, outTangentPosition);
            
            Handles.DrawSolidDisc(inTangentPosition, (inTangentPosition - camPosition).normalized, 1);
            Handles.DrawSolidDisc(outTangentPosition, (outTangentPosition - camPosition).normalized, 1);
        }

        Handles.color = handlesColor;
    }
    
    void DrawAnchorsHandlers()
    {
        if(!isEditMode)
            return;

        var anchors = Spline.GetAnchors();
        
        EditorGUI.BeginChangeCheck();
        
        for (var i = 0; i < anchors.Length; i++)
        {
            var anchor = anchors[i];
            Vector3 position = Handles.PositionHandle(anchor.Position, Quaternion.identity);
            Vector3 inTangent = Handles.PositionHandle(anchor.InTangent + anchor.Position, Quaternion.identity);
            Vector3 outTangent = Handles.PositionHandle(anchor.OutTangent + anchor.Position, Quaternion.identity);

            Spline.SetAnchor(new Spline.Anchor(position, inTangent-position, outTangent-position), i);
        }
        
        if(EditorGUI.EndChangeCheck())
            Spline.Rebuild();
    }

    void DrawSelectedHandler()
    {
        if(isEditMode || selectedAnchor < 0)
            return;
        
        EditorGUI.BeginChangeCheck();
        
        Spline.Anchor anchor = Spline.GetAnchor(selectedAnchor);
        
        Vector3 position = Handles.PositionHandle(anchor.Position, Quaternion.identity);
        Vector3 inTangent = Handles.PositionHandle(anchor.InTangent + anchor.Position, Quaternion.identity);
        Vector3 outTangent = Handles.PositionHandle(anchor.OutTangent + anchor.Position, Quaternion.identity);

        Spline.SetAnchor(new Spline.Anchor(position, inTangent-position, outTangent-position), selectedAnchor);
        
        if(EditorGUI.EndChangeCheck())
            Spline.Rebuild();
    }
    
    #endregion
}
