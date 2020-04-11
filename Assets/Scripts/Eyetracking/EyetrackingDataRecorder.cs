using System.Collections;
using System.Collections.Generic;
using Tobii.XR;
using UnityEngine;
using ViveSR.anipal.Eye;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class EyetrackingDataRecorder : MonoBehaviour
{
    // Start is called before the first frame update
    private float _sampleRate;
    private List<EyeTrackingDataFrame> _recordedEyeTrackingData;
    private EyetrackingManager _eyetrackingManager;
    private Transform _hmdTransform;
    private bool recordingEnded;
    void Start()
    {
        
        _recordedEyeTrackingData= new List<EyeTrackingDataFrame>();
        
        _eyetrackingManager= EyetrackingManager.Instance;
        
        _sampleRate = _eyetrackingManager.GetSampleRate();
        _hmdTransform = _eyetrackingManager.GetHmdTransform();
    }
    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartRecording()
    {
        Debug.Log("recording started");
        recordingEnded = false;
        StartCoroutine(RecordEyeTrackingData());
        
    }

    public void StopRecording()
    {
        recordingEnded = true;
    }
    
    private IEnumerator RecordEyeTrackingData()
    {
        
        Debug.Log("starting recording...");
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
                
                RaycastHit hitInformation;        //this might be more precise with a sphere cast or similar, since the eyetracker is having some error.
                if (Physics.Raycast(gazeRayOrigin, gazeRayDirection, out hitInformation))
                {
                    dataFrame.HitObjectName = hitInformation.collider.name;
                    dataFrame.HitObjectPosition = hitInformation.collider.transform.position;
                    dataFrame.HitPointOnObject = hitInformation.point;
                }
                
            }

            if (eyeTrackingDataLocal.GazeRay.IsValid)
            {
                dataFrame.EyePosLocalCombined = eyeTrackingDataLocal.GazeRay.Origin;
                dataFrame.EyeDirLocalCombined = eyeTrackingDataLocal.GazeRay.Direction;
                dataFrame.LeftEyeIsBlinkingLocal = eyeTrackingDataLocal.IsLeftEyeBlinking;
                dataFrame.RightEyeIsBlinkingLocal = eyeTrackingDataLocal.IsRightEyeBlinking;
            }

            dataFrame.TimeStamp = eyeTrackingDataWorld.Timestamp;
            
            dataFrame.HMDposition = _hmdTransform.position;

            dataFrame.NoseVector =  _hmdTransform.forward;

            Debug.Log("finished DataFrame");
            _recordedEyeTrackingData.Add(dataFrame);
            
            yield return new WaitForSeconds(_sampleRate);
        }
        
    }
}
