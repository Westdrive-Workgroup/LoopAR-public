using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Declares a point where a pedestrian or car spawns
/// </summary>
[ExecuteInEditMode]
public class SpawnPoint : MonoBehaviour {
    [SerializeField]
    [Space]
    [Header("Path Setup")]
    public BezierSplines path;
    public Vector3 pointPosition;
    public float percentageGone;
    public float duration;
    public bool isLooping;
    [Space]
    [Header("Apperance")]
    public float radius = 1;
    public bool visibleWhenNotSelected = false;

    public void setPosition(Vector3 position)
    {
        pointPosition = position;
        transform.position = pointPosition;
    }
    public void Start()
    {
        transform.position = pointPosition;
    }
}
