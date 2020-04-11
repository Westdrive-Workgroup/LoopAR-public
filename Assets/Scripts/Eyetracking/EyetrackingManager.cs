using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyetrackingManager : MonoBehaviour
{
    public static EyetrackingManager Instance { get; private set; }

    public int sampleRate;

    private EyeTrackingDataRecorder _eyeTrackingRecorder;
    private void Awake()
    {
        //singleton pattern a la Unity
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);         //the Traffic Manager should be persitent by changing the scenes maybe change it on the the fly
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _eyeTrackingRecorder = GetComponent<EyeTrackingDataRecorder>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            _eyeTrackingRecorder.StartRecording();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            _eyeTrackingRecorder.StopRecording();
        }
    }
}
