using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tobii.XR;
using UnityEngine;
using UnityEngine.SceneManagement;
using ViveSR.anipal.Eye;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class EyetrackingDataRecorder : MonoBehaviour
{
    // Start is called before the first frame update
    private float _sampleRate;
    private List<EyeTrackingDataFrame> _recordedEyeTrackingData;
    private List<float> _frameRates;
    private EyetrackingManager _eyetrackingManager;
    private Transform _hmdTransform;
    private bool recordingEnded;
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        _recordedEyeTrackingData= new List<EyeTrackingDataFrame>();
        
        _eyetrackingManager= EyetrackingManager.Instance;
        
        _sampleRate = _eyetrackingManager.GetSampleRate();
        _hmdTransform = _eyetrackingManager.GetHmdTransform();
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.P))
        {
            Visualisation();
            Debug.Log("<color=green>Visualisation activated!</color>");
        }*/
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)  // generally I am not proud of this call, but seems necessary for the moment.
    {
        _hmdTransform = _eyetrackingManager.GetHmdTransform();        //refresh the HMD transform after sceneload;
        //Debug.Log("hello new World");
    }

    
    public void StartRecording()
    {
        recordingEnded = false;
        StartCoroutine(RecordEyeTrackingData());
    }

    public void StopRecording()
    {
        recordingEnded = true;
    }

    public void ClearEyeTrackingDataRecordings()
    {
        _recordedEyeTrackingData.Clear();
    }
    
    private IEnumerator RecordEyeTrackingData()
    {
        int frameCounter = new int();
        Debug.Log("<color=green>Start recording...</color>");
        
        _frameRates = new List<float>();
        
        while (!recordingEnded)
        {
            EyeTrackingDataFrame dataFrame = new EyeTrackingDataFrame();
            var eyeTrackingDataWorld = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.World);
            var eyeTrackingDataLocal = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.Local);

            if (eyeTrackingDataWorld.GazeRay.IsValid)
            {
                Vector3 gazeRayOrigin = eyeTrackingDataWorld.GazeRay.Origin;
                Vector3 gazeRayDirection = eyeTrackingDataWorld.GazeRay.Direction;
                dataFrame.TobiiTimeStamp = eyeTrackingDataWorld.Timestamp;
                dataFrame.EyePosWorldCombined = gazeRayOrigin;
                dataFrame.EyeDirWorldCombined = gazeRayDirection;

                dataFrame.LeftEyeIsBlinkingWorld = eyeTrackingDataWorld.IsLeftEyeBlinking;
                dataFrame.RightEyeIsBlinkingWorld = eyeTrackingDataWorld.IsRightEyeBlinking;

                dataFrame.hitObjects = GetHitObjectsFromGaze(gazeRayOrigin, gazeRayDirection);
            }

            if (eyeTrackingDataLocal.GazeRay.IsValid)
            {
                dataFrame.EyePosLocalCombined = eyeTrackingDataLocal.GazeRay.Origin;
                dataFrame.EyeDirLocalCombined = eyeTrackingDataLocal.GazeRay.Direction;
                dataFrame.LeftEyeIsBlinkingLocal = eyeTrackingDataLocal.IsLeftEyeBlinking;
                dataFrame.RightEyeIsBlinkingLocal = eyeTrackingDataLocal.IsRightEyeBlinking;
            }

            dataFrame.UnixTimeStamp = TimeManager.Instance.GetCurrentUnixTimeStamp();
            
            dataFrame.FPS = SavingManager.Instance.GetCurrentFPS();
            
            dataFrame.HmdPosition = EyetrackingManager.Instance.GetHmdTransform().position;

            dataFrame.NoseVector =  EyetrackingManager.Instance.GetHmdTransform().forward;
            
            _frameRates.Add(dataFrame.FPS);
            _recordedEyeTrackingData.Add(dataFrame);
            frameCounter++;
            
            yield return new WaitForSeconds(_sampleRate);
        }
    }


    private List<HitObjectInfo> GetHitObjectsFromGaze(Vector3 gazeOrigin, Vector3 gazeDirection)
    {
        RaycastHit[] hitColliders = Physics.RaycastAll(gazeOrigin, gazeDirection);
        
        List<HitObjectInfo> hitObjectInfoList= new List<HitObjectInfo>();
        
        foreach (var colliderhit in hitColliders)
        {
                    
            HitObjectInfo hitInfo = new HitObjectInfo();
            hitInfo.ObjectName = colliderhit.collider.gameObject.name;
            hitInfo.HitObjectPosition = colliderhit.collider.transform.position;
            hitInfo.HitPointOnObject = colliderhit.point;
            hitObjectInfoList.Add(hitInfo);
        }

        return hitObjectInfoList;
    }

    private List<HitObjectInfo> GetFirstHitObjectFromGaze(Vector3 gazeOrigin, Vector3 gazeDirection, float distance)
    {
        RaycastHit hit;
        bool hitColliders = Physics.Raycast(gazeOrigin, gazeDirection, out hit, distance);
        
        List<HitObjectInfo> hitObjectInfoList= new List<HitObjectInfo>();

        if (hitColliders)
        {
            HitObjectInfo hitInfo = new HitObjectInfo();
            hitInfo.ObjectName = hit.collider.gameObject.name;
            hitInfo.HitObjectPosition = hit.collider.transform.position;
            hitInfo.HitPointOnObject = hit.point;
            hitObjectInfoList.Add(hitInfo);
        }

        return hitObjectInfoList;
    }


    public List<EyeTrackingDataFrame> GetDataFrames()
    {
        if (recordingEnded)
        {
            return _recordedEyeTrackingData;
        }
        else
        {
            throw new Exception("Eyetracking Data Recording has not been finished");
        }
    }

    public float GetAverageFrameRate()
    {
        return _frameRates.Average();
    }

    private void Visualisation()
    {
        List<EyeTrackingDataFrame> dataFrames = GetDataFrames();

        foreach (var dataFrame in dataFrames)
        {
            if (dataFrame.hitObjects != null)
            {
                foreach (var item in dataFrame.hitObjects)
                {
                    Debug.Log(item.ObjectName);
                    Debug.DrawLine(dataFrame.HmdPosition, item.HitPointOnObject, Color.red, 60f);
                }
            }
        }
    }
}
