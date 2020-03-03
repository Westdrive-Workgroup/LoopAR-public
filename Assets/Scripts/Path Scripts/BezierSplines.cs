using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// Splits the paths in small "splines" and uses the Bezier transformation to smoothe the path
/// </summary>
public class BezierSplines : MonoBehaviour
{
    [Space]
    [Header("Path Setting for cars on the path")]
    public float defaultDuration = 100f;
    [SerializeField]
    private Vector3[] points;
    private Quaternion[] pointsOrientation;
    public Quaternion[] Orientations{

        get
        {
            return pointsOrientation;
        }
    }
    [SerializeField]
    private BezierControlPointMode[] modes;
    [SerializeField]
    private bool showGizmosPermanemt;
    private bool showGizmos;
    public string pathMode;
    public float duration;
    public float speed;
    private bool loop;
    public Color curveColor = Color.white;
    public bool visibleWhenNotSelected;
    public float gizmosradius = 1f;
    public float newCurveDistancing = 1f;
    // decides if gizmos should be shown permanent
    public bool ShowGizmosPermanent
    {
        get
        {
            return showGizmosPermanemt;
        }
        set
        {
            showGizmosPermanemt = value;

        }
    }

    // decides if gizmos should be shown 
    public bool ShowGizmos
    {
        get
        {
            return showGizmos;
        }
        set
        {
            showGizmos = value;

        }
    }
    public bool Loop
    {
        get
        {
            return loop;
        }
        set
        {
            loop = value;
            if (value == true)
            {
                modes[modes.Length - 1] = modes[0];
                SetControlPoint(0, points[0]);
            }
        }
    }
    //counts the control points
    public int ControlPointCount
    {
        get
        {
            return points.Length;
        }
    }
    //returns the last index
    public int returnLastIndex()
    {
        return (points.Length - 1);
    }
    public Vector3 GetControlPoint(int index)
    {
        EnforceMode(index);
        return points[index];

    }
    // Returns the orientation of the control point
    public Quaternion GetControlPointOrientation(int index)
    {
        try
        {
            EnforceMode(index);

            return pointsOrientation[index];
        }
        catch(Exception ex)
        {
            _fixOldVersion();
            return pointsOrientation[index];
        }
       

    }
    // changes the Gizmo color to the curve color
    public void OnDrawGizmos()
    {
        if (showGizmosPermanemt)
        {
            foreach (Vector3 point in points)
            {
                Gizmos.color = curveColor;
                Gizmos.DrawWireSphere(transform.TransformPoint(point), gizmosradius);
            }
        }
    }

    // changes the Gizmo color to the curve color
    public void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            foreach (Vector3 point in points)
            {
                Gizmos.color = curveColor;
                Gizmos.DrawWireSphere(transform.TransformPoint(point), gizmosradius);
            }
        }
    }
    // can change the control point rotation
    public void SetControlPointsRotation(int index, Quaternion newOrientation)
    {
        pointsOrientation[index] = newOrientation;
    }
    // changes the control point position 
    public void SetControlPoint(int index, Vector3 point)
    {

        if (index % 3 == 0)
        {
            Vector3 delta = point - points[index];
            if (loop)
            {
                if (index == 0)
                {
                    points[1] += delta;
                    points[points.Length - 2] += delta;
                    points[points.Length - 1] = point;
                }
                else if (index == points.Length - 1)
                {
                    points[0] = point;
                    points[1] += delta;
                    points[index - 1] += delta;
                }
                else
                {
                    points[index - 1] += delta;
                    points[index + 1] += delta;
                }
            }
            else
            {
                if (index > 0)
                {
                    points[index - 1] += delta;
                }
                if (index + 1 < points.Length)
                {
                    points[index + 1] += delta;
                }
            }
        }
        points[index] = point;
        EnforceMode(index);
    }
    //  takes care, that if you change a control point in a curve the whole curve has the same modus
    private void EnforceMode(int index)
    {
        int modeIndex = (index + 1) / 3;
        BezierControlPointMode mode = modes[modeIndex];
        if (mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Length - 1))
        {
            return;
        }

        int middleIndex = modeIndex * 3;
        int fixedIndex, enforcedIndex;
        if (index <= middleIndex)
        {
            fixedIndex = middleIndex - 1;
            if (fixedIndex < 0)
            {
                fixedIndex = points.Length - 2;
            }
            enforcedIndex = middleIndex + 1;
            if (enforcedIndex >= points.Length)
            {
                enforcedIndex = 1;
            }
        }
        else
        {
            fixedIndex = middleIndex + 1;
            if (fixedIndex >= points.Length)
            {
                fixedIndex = 1;
            }
            enforcedIndex = middleIndex - 1;
            if (enforcedIndex < 0)
            {
                enforcedIndex = points.Length - 2;
            }
        }

        Vector3 middle = points[middleIndex];
        Vector3 enforcedTangent = middle - points[fixedIndex];
        if (mode == BezierControlPointMode.Aligned)
        {
            enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
        }
        points[enforcedIndex] = middle + enforcedTangent;
    }
    //counts the curves by dividing (all points -1) with three 
    public int CurveCount
    {
        get
        {
            return (points.Length - 1) / 3;
        }
    }
    //Gets a certain Point 
    public Vector3 GetPoint(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }
        return transform.TransformPoint(Bezier.GetPoint(
            points[i], points[i + 1], points[i + 2], points[i + 3], t));
    }
    //Gets the first derivate of a certain point
    public Vector3 GetVelocity(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }
        return transform.TransformPoint(Bezier.GetFirstDerivative(
            points[i], points[i + 1], points[i + 2], points[i + 3], t)) - transform.position;
    }
    //normalizes the velocity
    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }
    // changes the the orientation back to the start identity
    public void Reset()
    {

        transform.localRotation = Quaternion.identity;
        points = new Vector3[] {
            new Vector3(0f, 0f, 1f),
            new Vector3(0f, 0f, 2f),
            new Vector3(0f, 0f, 3f),
            new Vector3(0f, 0f, 4f)
        };
        pointsOrientation = new Quaternion[] {
             Quaternion.identity,
             Quaternion.identity,
             Quaternion.identity,
             Quaternion.identity
        };
        modes = new BezierControlPointMode[] {
            BezierControlPointMode.Free,
            BezierControlPointMode.Free
        };
        transform.hasChanged = false;
    }
    // uses an euler function to adjust the angles 
    public Quaternion adjustAngle(Quaternion orientation)
    {
        Vector3 orientationAngles = orientation.eulerAngles;
        Vector3 transformOrientationAngles = transform.rotation.eulerAngles;
        Vector3 correctedAngles = orientationAngles - transformOrientationAngles;
        return Quaternion.Euler(correctedAngles);
    }
    // Deletes a curve by shorting the array by three
    public void DeleteCurve()
    {
        Array.Resize(ref points, points.Length - 3);
        EnforceMode(points.Length - 1);

    }
    // Adds a new curve by adding 3 new Cotrollpoints to the path
    public void AddCurve()
    {
        Vector3 point = points[points.Length - 1];
        Quaternion pointOrientation = adjustAngle(pointsOrientation[points.Length - 1]);
        Array.Resize(ref points, points.Length + 3);
        Array.Resize(ref pointsOrientation, pointsOrientation.Length + 3);
        //Debug.Log(pointOrientation * Vector3.forward);
        point += (pointOrientation * Vector3.forward) * newCurveDistancing;
        points[points.Length - 3] = point;
        pointsOrientation[pointsOrientation.Length - 3] = pointOrientation;
        point += (pointOrientation * Vector3.forward) * newCurveDistancing;
        points[points.Length - 2] = point;
        pointsOrientation[pointsOrientation.Length - 2] = pointOrientation;
        point += (pointOrientation * Vector3.forward) * newCurveDistancing;
        points[points.Length - 1] = point;
        pointsOrientation[pointsOrientation.Length - 1] = pointOrientation;

        Array.Resize(ref modes, modes.Length + 1);
        modes[modes.Length - 1] = modes[modes.Length - 2];
        EnforceMode(points.Length - 4);

        if (loop)
        {
            points[points.Length - 1] = points[0];
            modes[modes.Length - 1] = modes[0];
            EnforceMode(0);
        }
    }// returns the mode of a certain control point
    public BezierControlPointMode GetControlPointMode(int index)
    {
        return modes[(index + 1) / 3];
    }
    // Sets the mode of a certain Controll point
    public void SetControlPointMode(int index, BezierControlPointMode mode)
    {
        int modeIndex = (index + 1) / 3;
        modes[modeIndex] = mode;
        if (loop)
        {
            if (modeIndex == 0)
            {
                modes[modes.Length - 1] = mode;
            }
            else if (modeIndex == modes.Length - 1)
            {
                modes[0] = mode;
            }
        }
        EnforceMode(index);
    }
    // Runs through all points and changes their orientation back to the identity
    public void _fixOldVersion()
    {
        //Debug.Log("fixing old version");
        //Debug.Log("points.Length");
        pointsOrientation = new Quaternion[points.Length];
        int index = 0;
        foreach (Vector3 point in points)
        {
            pointsOrientation[index] = Quaternion.identity;
            index++;
        }
    }
    // Runs through all points orientations and changes their rotation
    public void _editModeUpdate()
    {
       // Debug.Log("changed");
        
        for (int index = 0 ; index < pointsOrientation.Length; index++)
        {
            pointsOrientation[index] =  transform.rotation;
        }
        transform.hasChanged = false;
    }


}
