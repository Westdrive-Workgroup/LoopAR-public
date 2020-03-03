using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Moves objects along a pathway
/// </summary>
public class SimpleFollower : MonoBehaviour {
   
   // public GameObject car;
    [Space]
    [Header("Path Settings")]
    public BezierSplines path;
    public bool isLoop = false;
    [Tooltip("initial duration for the car to complete the path")]
    public float duration = 0;
    public bool usePathDefaultDuration = true;
    private float[] pathLengthTable;
    private Vector3[] pathNodesTable;
    private float increamentUnit = 0;
    public float startPecentage = 0;
    private float pastDuration;
    // Cars progress on the path
    private float progress;
    [Tooltip("Pedestrian head follow the direction of the path")]
    public bool lookForward = true;
    [Tooltip("Sets if the Pedestrian is going forward in the path or going back from the end of the path")]
    public bool goingForward = true;

    private Vector3 positionToBe;
    private Vector3 directionToHave;

    
    // starts the initialize method and saves the startPercentage in the float progress
    void Start()
    {
        _initialize();       
        progress = startPecentage;
    }
   
    // Assorts all moveable objects their startposition corresponding to the startPercentage
    private void _initialize()
    {
        
        positionToBe = path.GetPoint(startPecentage);
        directionToHave = positionToBe + path.GetDirection(startPecentage);
        if (this.CompareTag("Tracker"))
        {
            transform.position = positionToBe;
            transform.LookAt(directionToHave);
        }
        else
        {
            transform.localPosition = positionToBe;
            transform.LookAt(directionToHave);
        }
        if (usePathDefaultDuration)
            duration = path.duration;
        increamentUnit = Time.fixedDeltaTime / duration;
      

    }
    // changes the position by using the progress
    void MoveStep()
    {
        if (goingForward)
        {
            progress += increamentUnit;
            if (progress > 1f)
            {
                if (!isLoop)
                {
                    progress = 1f;
                }
                else
                {
                    progress -= 1f;
                }
            }
        }
        else
        {
            progress -= increamentUnit;
            if (progress < 0f)
            {
                progress = -progress;
                goingForward = true;
            }
        }

       
        Vector3 position = path.GetPoint(progress);
        positionToBe = position;
        if (lookForward)
        {
            directionToHave = position + path.GetDirection(progress);
        }
    }
    // calls the method MoveStep once per frame
    private void FixedUpdate()
    {
        MoveStep();
    }
    // At the beginning of each frame changes the position of the object to the positionToBe
    void Update()
    {
        if (this.CompareTag("Event Object"))
        {   
            transform.position = positionToBe;
            transform.LookAt(directionToHave);
        }
        else
        {   
            transform.localPosition = positionToBe;
            transform.LookAt(directionToHave);
        }               
    }

}


