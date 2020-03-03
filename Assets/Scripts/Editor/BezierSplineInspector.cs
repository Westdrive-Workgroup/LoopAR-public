using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(BezierSplines))]
public class BezierSplineInspector : Editor
{

    private const int lineSteps = 10;
    private const float directionScale = 0.5f;
    private const int stepsPerCurve = 10;
    private BezierSplines spline;
    private Transform handleTransform;
    private Quaternion handleRotation;
    
    private const float handleSize = 0.04f;
    private const float pickSize = 0.06f;

    private int selectedIndex = -1;
    private static Color[] modeColors = {
        Color.white,
        Color.yellow,
        Color.cyan
    };
    //string[] _choices = new[] { "Pedestrian", "Car" };
    //int _choiceIndex = 0;
    private void OnScene(SceneView sceneview)
    {
        OnSceneGUI();
    }
    
    private void OnSceneGUI()
    {
        spline = target as BezierSplines;
        handleTransform = spline.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;
        
        Vector3 p0 = ShowPoint(0);
        for (int i = 1; i < spline.ControlPointCount; i += 3)
        {
            Vector3 p1 = ShowPoint(i);
            Vector3 p2 = ShowPoint(i + 1);
            Vector3 p3 = ShowPoint(i + 2);

            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            Handles.DrawBezier(p0, p3, p1, p2, spline.curveColor, null, 2f);
            p0 = p3;
        }
        if (spline.visibleWhenNotSelected)
        {
            SceneView.onSceneGUIDelegate -= OnScene;
            SceneView.onSceneGUIDelegate += OnScene;
        }
        if (!spline.visibleWhenNotSelected)
        {
            SceneView.onSceneGUIDelegate -= OnScene;
        }
    }

    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = spline.GetPoint(0f);
        Handles.DrawLine(point, point + spline.GetDirection(0f) * directionScale);
        int steps = stepsPerCurve * spline.CurveCount;
        for (int i = 1; i <= steps; i++)
        {
            point = spline.GetPoint(i / (float)steps);
            Handles.DrawLine(point, point + spline.GetDirection(i / (float)steps) * directionScale);
        }
    }

    private Vector3 ShowPoint(int index)
    {
        Vector3 point = handleTransform.TransformPoint(spline.GetControlPoint(index));
        Quaternion Orientation = Quaternion.identity;
        //Quaternion pointOrientation = spline.GetControlPointOrientation(index);
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            spline.GetControlPointOrientation(index) : Quaternion.identity;
        //if (spline.transform.hasChanged)
        //{
        //    spline._editModeUpdate();
        //}
        float size = HandleUtility.GetHandleSize(point);
        
        if (index == 0)
        {
           
            size *= 2f;
        }
        Handles.color = modeColors[(int)spline.GetControlPointMode(index)];
        if (index % 3 == 0)
            Handles.color = Color.green;

        if (Handles.Button(point, handleRotation, size * handleSize, size * pickSize, Handles.DotHandleCap))
        {
            selectedIndex = index;
            Repaint();
        }
        Handles.color = Color.white;
        
        if (selectedIndex == index)
        {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (selectedIndex == spline.returnLastIndex())
            {
                Orientation = Handles.DoRotationHandle(handleRotation, point);
                

            }
           
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spline, "Move Point");
                spline.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
                if(Orientation != Quaternion.identity)
                {

                    Undo.RecordObject(spline, "Orientation of Path Section Changed");
                    spline.SetControlPointsRotation(index , Orientation);
                }
                //Debug.Log(index);
                EditorUtility.SetDirty(spline);
            }
        }
        return point;
    }
    public override void OnInspectorGUI()
    {
        
        spline = target as BezierSplines;
        //if(spline.Orientations == null)
        //{
        //    spline._fixOldVersion();
        //}
        EditorGUI.BeginChangeCheck();
        bool visible = EditorGUILayout.Toggle("Always Visible", spline.visibleWhenNotSelected);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Toggle Visiblity");
            EditorUtility.SetDirty(spline);
            spline.visibleWhenNotSelected = visible;
        }
        EditorGUI.BeginChangeCheck();
        Color splineColor = EditorGUILayout.ColorField("Spline Color", spline.curveColor);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Spline color change");
            EditorUtility.SetDirty(spline);
            spline.curveColor = splineColor;
        }
        EditorGUI.BeginChangeCheck();
        float duration = EditorGUILayout.FloatField("Path traverse duration", spline.duration);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Spline duration changed");
            EditorUtility.SetDirty(spline);
            spline.duration = duration;
        }
        EditorGUI.BeginChangeCheck();
        float speed = EditorGUILayout.FloatField("Path traverse speed", spline.speed);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Spline speed changed");
            EditorUtility.SetDirty(spline);
            spline.speed = speed;
        }

        EditorGUI.BeginChangeCheck();
        bool loop = EditorGUILayout.Toggle("Loop", spline.Loop);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Toggle Loop");
            EditorUtility.SetDirty(spline);
            spline.Loop = loop;
        }
        EditorGUI.BeginChangeCheck();
        float newCurveDistancing = EditorGUILayout.FloatField("nodes distancing by new curve", spline.newCurveDistancing);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Spline new line distancing changed");
            EditorUtility.SetDirty(spline);
            spline.newCurveDistancing = newCurveDistancing;
        }
        EditorGUI.BeginChangeCheck();
        float gizmosradius = EditorGUILayout.FloatField("helper gizmos radius", spline.gizmosradius);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Spline gizmos radius changed");
            EditorUtility.SetDirty(spline);
            spline.gizmosradius = gizmosradius;
        }
        EditorGUI.BeginChangeCheck();
        bool showGizmosPermanent = EditorGUILayout.Toggle("Permanently Show gizmos", spline.ShowGizmosPermanent);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Toggle permanent gizmos visibility");
            EditorUtility.SetDirty(spline);
            spline.ShowGizmosPermanent = showGizmosPermanent;
        }
        EditorGUI.BeginChangeCheck();
        bool showGizmos = EditorGUILayout.Toggle("Show gizmos on selection", spline.ShowGizmos);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Toggle gizmos visibility");
            EditorUtility.SetDirty(spline);
            spline.ShowGizmos = showGizmos;
        }
        if (selectedIndex >= 0 && selectedIndex < spline.ControlPointCount)
        {
            DrawSelectedPointInspector();
        }
        //_choiceIndex = EditorGUILayout.Popup(_choiceIndex, _choices);
        //Undo.RecordObject(spline, "bspline mode change");
        //spline.pathMode = _choices[_choiceIndex];
        //EditorUtility.SetDirty(spline);
        if (GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(spline, "Add Curve");
            spline.AddCurve();
            EditorUtility.SetDirty(spline);
        }
        if (GUILayout.Button("Remove Curve"))
        {
            Undo.RecordObject(spline, "Remove Curve");
            spline.DeleteCurve();
            EditorUtility.SetDirty(spline);
        }


    }
    private void DrawSelectedPointInspector()
    {
        GUILayout.Label("Selected Point");
        EditorGUI.BeginChangeCheck();
        Vector3 point = EditorGUILayout.Vector3Field("Position", spline.GetControlPoint(selectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Move Point");
            EditorUtility.SetDirty(spline);
            spline.SetControlPoint(selectedIndex, point);
        }
        
        EditorGUI.BeginChangeCheck();
        BezierControlPointMode mode = (BezierControlPointMode)
            EditorGUILayout.EnumPopup("Mode", spline.GetControlPointMode(selectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Change Point Mode");
            spline.SetControlPointMode(selectedIndex, mode);
            EditorUtility.SetDirty(spline);
        }
    }
}