using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Assorts the pedestrian with the spawnpoints and moves them
/// </summary>
public class CharacterManager : MonoBehaviour {
    Animator anim;  // needed for playing the animation
    /*[Header("Internals")]
    private string objID;
    public string objectID
    {
        get { return objID; }
        set { objID = value; }
    }
    private Dictionary<int, PositionRotationType> dataPoints;*/
    //NavMeshAgent agent;
    [Space]
    [Header("Debug - Calculation Logs")]
    public bool dumpInitialCalculations = false;
    [Space]
    [Header("Path Settings")]
    public BezierSplines path;    // the path which the pedestrians follow, needed.
    public bool isLoop = false;    // is the path a loop or not. needed
    [Tooltip("initial duration for the car to complete the path")]
    public float duration = 20f;    // needed
    public bool usePathDefaultDuration = true;    /// <summary>
                                                  /// 
                                                  /// </summary>
    private float[] pathLengthTable;    /// <summary>
                                        /// 
                                        /// </summary>
    private Vector3[] pathNodesTable; /// <summary>
                                      /// 
                                      /// </summary>
    private float _increamentUnit = 0; /// <summary>
                                      /// 
                                      /// </summary>
    public float startPecentage = 0; /// <summary>
                                     /// 
                                     /// </summary>
    private float _pastDuration;
    // Cars progress on the path
    private float _progress; /// <summary>
                            /// 
                            /// </summary>
    [Tooltip("Pedestrian head follow the direction of the path")]
    public bool lookForward = true; /// <summary>
                                    /// 
                                    /// </summary>
    [Tooltip("Sets if the Pedestrian is going forward in the path or going back from the end of the path")]
    public bool goingForward = true; /// <summary>
                                     /// 
                                     /// </summary>
    private Vector3 _positionToBe; /// <summary>
                                  /// 
                                  /// </summary>
    private Vector3 _directionToHave; /// <summary>
                                     /// 
                                     /// </summary>
    Vector2 _smoothDeltaPosition = Vector2.zero;
    Vector2 _velocity = Vector2.zero;   
    float _currentAngularVelocity;    
    bool _shouldMove = false; /// <summary>
                             /// 
                             /// </summary>
    private string _initialGoal;
    Vector3 _lastFacing;
    private int _defaultLayerID;
    private int _avoidableLayerID;

    // starts to mesh the skin after this class is called
    /*private void Awake()
    {
        dataPoints = new Dictionary<int, PositionRotationType>();
        SkinnedMeshRenderer [] meshes = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach(SkinnedMeshRenderer mesh in meshes)
        {
            mesh.enabled = true;
            
        }
    }*/
    // calls the method _initialize and saves the startPercentage in the progress
    void Start()
    {
        // defaultLayerID = gameObject.layer;
        // avoidableLayerID =  UnityEngine.LayerMask.NameToLayer("avoidable");
        anim = GetComponent<Animator>();
        
        _initialize();
       
        _progress = startPecentage;
    }
    // calculates the distance between spawnpoints
    public float[] _calculateLengthTableInfo()
    {
        float totalLength = 0f;
        float arrayLength = 1f / (Time.fixedDeltaTime / duration);
        float[] array = new float[Mathf.FloorToInt(arrayLength)];
        array[0] = 0f;
        Vector3 previousPoint = path.GetPoint(0);
        for (int index = 0; index < array.Length; index++)
        {
            float t = ((float)index) / (array.Length - 1);
            Vector3 newPoint = path.GetPoint(t);
            float distaceDifference = (previousPoint - newPoint).magnitude;
            totalLength += distaceDifference;
            array[index] = totalLength;
            previousPoint = newPoint;
        }
        return array;
    }
    // calculates the nodes Table info
    public Vector3[] _calculateNodesTableInfo()
    {

        float arrayLength = 1f / (Time.fixedDeltaTime / duration);
        Vector3[] nodes = new Vector3[Mathf.FloorToInt(arrayLength)];
        nodes[0] = path.GetPoint(0);
        Vector3 previousPoint = path.GetPoint(0);
        for (int index = 1; index < nodes.Length; index++)
        {
            float t = ((float)index) / (nodes.Length - 1);
            Vector3 newPoint = path.GetPoint(t);
         
            nodes[index] = newPoint;
            previousPoint = newPoint;
        }
        return nodes;
    }
    //initializes the different Events and
    private void _initialize()
    {
        _initialGoal = this.transform.parent.name;
        /*if (this.CompareTag("Event Object"))
        {
            switch (this.transform.parent.gameObject.name)
            {
                case "Event1.1":
                    anim.SetInteger("Condition", 1);
                    break;
                case "Event2.1":
                    anim.SetInteger("Condition", 1);
                    break;
                case "Event2.2":
                    anim.SetInteger("Condition", 2);
                    break;
                case "Event3.1":
                    anim.SetInteger("Condition", 3);
                    break;
                case "Event3.2":
                    anim.SetInteger("Condition", 4);
                    break;
                case "Event3.4":
                    anim.SetInteger("Condition", 1);
                    break;
                case "Event4.1 (MSW)":
                    anim.SetInteger("Condition", 1);
                    break;
                case "Event4.3 (MSW)":
                    anim.SetInteger("Condition", 3);
                    break;
                default:

                    break;
            }
            anim.enabled = true;
        }
        else
        {
            anim.SetBool("move", true);
            anim.enabled = true;
        }*/
        
        anim.SetBool("move", true);
        anim.enabled = true;
        
        _positionToBe = path.GetPoint(startPecentage);
        _directionToHave = _positionToBe + path.GetDirection(startPecentage);
        /*if (this.CompareTag("Event Object"))
        {
            transform.position = positionToBe;
            transform.LookAt(directionToHave);
        }
        else
        {
            transform.localPosition = positionToBe;
            transform.LookAt(directionToHave);
        }*/
        
        transform.localPosition = _positionToBe;
        transform.LookAt(_directionToHave);
        
        // if (usePathDefaultDuration)
        // duration = path.duration; //todo fix the getter. Duration is always 0
        _increamentUnit = Time.fixedDeltaTime / duration;
        pathLengthTable = _calculateLengthTableInfo();
        pathNodesTable = _calculateNodesTableInfo();
        /*if(WestdriveSettings.SimulationMode == mode.record)
            StartCoroutine(recorder());*/
        /*if (WestdriveSettings.SimulationMode == mode.simulate && this.CompareTag("Event Object"))
        {
            StartCoroutine(EventRecorder());
        }*/
    }
    //is called once per frame and advances an object along its path and orientation
    void MoveStep()
    {
        if (goingForward)
        {
            _progress += _increamentUnit;
            if (_progress > 1f)
            {
                if (!isLoop)
                {
                    _progress = 1f;
                }
                else
                {
                    _progress -= 1f;
                }
            }
        }
        else
        {
            _progress -= _increamentUnit;
            if (_progress < 0f)
            {
                _progress = -_progress;
                goingForward = true;
            }
        }

       
        Vector3 position = path.GetPoint(_progress);
        _positionToBe = position;
        if (lookForward)
        {
            _directionToHave = position + path.GetDirection(_progress);
        }
    }
    //calls the method MoveStep once per frame
    private void FixedUpdate()
    {
        MoveStep();
    }
    //updates the position and orientation of event objects
    void Update()
    {
        _shouldMove = true;
  
        /*if(this.CompareTag("Event Object"))
        {   
            transform.position = positionToBe;
            transform.LookAt(directionToHave);
        }
        else
        {   
            transform.localPosition = positionToBe;
            transform.LookAt(directionToHave);
        }*/

        transform.localPosition = _positionToBe;
        transform.LookAt(_directionToHave);
        
    }
    // Activates if an event related objects enters the collider and destroys it if intended
    /*private void OnTriggerEnter(Collider other)
    {
        if (this.CompareTag("Event Object"))
        {
           
            if (other.CompareTag("End Event"))
            {
                this.transform.parent.GetComponent<EventHandler>().needMotorControl = false;
            }
            if (other.CompareTag("Destroy Object"))
            {
                if (WestdriveSettings.SimulationMode == mode.simulate)
                {
                    WestdriveSettings.EventData.Data.Add(this.transform.parent.gameObject.name,dataPoints);
                }
                this.transform.parent.gameObject.SetActive(false);
                Destroy(this.gameObject);
            }
        }
        else if (this.CompareTag("Pedestrian"))
        {
            if (other.CompareTag("Cross Walk"))
            {
                //Debug.Log("Entering Crosswalk");
                gameObject.layer = avoidableLayerID;
            }
        }
    }
    // Activates if pedestrian exits the collider and sets the gameobject layer to default
    private void OnTriggerExit(Collider other)
    {
        if (this.CompareTag("Pedestrian"))
        {
            if (other.CompareTag("Cross Walk"))
            {
               // Debug.Log("Entering Exiting");
                gameObject.layer = defaultLayerID;
            }
        }
    }
    // this section is the code related to recording positions
    private IEnumerator recorder()
    {
        while (true)
        {
            int frameCount = Time.frameCount;
            PositionRotationType dataPoint = new PositionRotationType();
            dataPoint.position = transform.position;
            dataPoint.rotaion = transform.rotation;
            if (!dataPoints.ContainsKey(frameCount))
                dataPoints.Add(Time.frameCount, dataPoint);
            else
                dataPoints[frameCount] = dataPoint;
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator EventRecorder()
    {
        while (true)
        {
            int frameCount = Time.frameCount - WestdriveSettings.frameCorrection;
            PositionRotationType dataPoint = new PositionRotationType();
            dataPoint.position = transform.position;
            dataPoint.rotaion = transform.rotation;
            if (!dataPoints.ContainsKey(frameCount))
                dataPoints.Add(frameCount, dataPoint);
            else
                dataPoints[frameCount] = dataPoint;
            yield return new WaitForEndOfFrame();
        }
    }
    // Returns the dataPoints
    public Dictionary<int, PositionRotationType> getData()
    {
        return dataPoints;
    }
    // Stops all coroutines
    

    private void OnDestroy()
    {
        StopAllCoroutines();
        
    }*/
}


