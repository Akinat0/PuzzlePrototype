using System;
using System.Linq;
using Puzzle;
using PuzzleScripts;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(EnemyMarker), true)]
public class EnemyMarkerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawProperties();
    }

    bool baseInspectorFoldout = true;
    EnemyMarker[] markers;
    
    void OnEnable()
    {
        markers = targets.Cast<EnemyMarker>().ToArray();
        
        if(markers.Length > 1)
            Array.Sort(markers, (m1, m2) => m1.time.CompareTo(m2.time));
    }
    
    void DrawProperties()
    {
        GUILayout.BeginVertical();

        DrawInverseSidesButton();
        DrawInverseSticksButton();
        
        DrawGroupEditor();

        foreach (EnemyMarker marker in markers)
            DrawProperty(marker);
        
        GUILayout.EndVertical();
    }

    void DrawProperty(EnemyMarker marker)
    {
        DrawDelimiter();
        DrawTime(marker);
        EnemyType enemyType = (EnemyType)EditorGUILayout.Popup("Enemy type", (int) marker.enemyParams.enemyType, Enum.GetNames(typeof(EnemyType)));
        marker.enemyParams.enemyType = enemyType;
        
        switch (enemyType)
        {
            case EnemyType.Puzzle:
                DrawPuzzleParameters(marker);
                break;
            case EnemyType.Shit:
                DrawShitParameters(marker);
                break;
            case EnemyType.Virus:
                DrawVirusParameters(marker);
                break;
        }
    }

    void DrawDelimiter()
    {
        if (markers.Length <= 1)
            return;
        
        GUILayout.Label("================");
    }

    void DrawGroupEditor()
    {
        baseInspectorFoldout = EditorGUILayout.Foldout(baseInspectorFoldout, "Group editor");
        
        if (baseInspectorFoldout)
            base.OnInspectorGUI();
    }
    
    void DrawInverseSidesButton()
    {
        if (GUILayout.Button("INVERSE SIDES"))
        {
            foreach (EnemyMarker marker in markers)
                marker.enemyParams.side = (Side) (((int) marker.enemyParams.side + 2) % 4);
        }
    }

    void DrawInverseSticksButton()
    {
        if (GUILayout.Button("INVERSE STICKS"))
        {
            foreach (EnemyMarker marker in markers)
                marker.enemyParams.stickOut = !marker.enemyParams.stickOut;
        }
    }
    
    void DrawPuzzleParameters(EnemyMarker marker)
    {
        DrawSpeed(marker);
        DrawSide(marker);
        DrawStickOut(marker);
    }

    void DrawShitParameters(EnemyMarker marker)
    {
        DrawSpeed(marker);
        DrawSide(marker);
    }

    void DrawVirusParameters(EnemyMarker marker)
    {
        DrawSpeed(marker);
        DrawSide(marker);
        DrawRadialPosition(marker);
    }


    void DrawTime(EnemyMarker marker)
    {
        marker.time = EditorGUILayout.DoubleField("Time", marker.time);
    }
    
    void DrawSpeed(EnemyMarker marker)
    {
        marker.enemyParams.speed = EditorGUILayout.FloatField("Speed", marker.enemyParams.speed);
    }

    void DrawSide(EnemyMarker marker)
    {
        marker.enemyParams.side = (Side) EditorGUILayout.Popup("Side", (int) marker.enemyParams.side, Enum.GetNames(typeof(Side)));
    }

    void DrawStickOut(EnemyMarker marker)
    {
        marker.enemyParams.stickOut = EditorGUILayout.Toggle("StickOut", marker.enemyParams.stickOut);
    }

    void DrawRadialPosition(EnemyMarker marker)
    {
        marker.enemyParams.radialPosition =
            EditorGUILayout.Slider("Radial Position", marker.enemyParams.radialPosition, 0, 359);
    }
}
