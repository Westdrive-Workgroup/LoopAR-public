using System.Collections;
using System.Collections.Generic;
using Tobii.XR;
using UnityEngine;
using ViveSR.anipal.Eye;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class EyeTrackingDataRecorder : MonoBehaviour
{
    // Start is called before the first frame update
    private float _sampleRate;
    private List<EyeTrackingDataFrame> _recordedEyeTrackingData;
    private Transform _hmdTransform;
    private bool recordingEnded;
    void Start()
    {
        _recordedEyeTrackingData= new List<EyeTrackingDataFrame>();

        _hmdTransform = Player.instance.hmdTransform; // here we might create a unnecessary dependency to Steam VR

        if (EyetrackingManager.Instance != null)
        {
            _sampleRate = EyetrackingManager.Instance.sampleRate;
        }
        
        else
        {
            Debug.Log("Error: an EyetrackingManager is missing ");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartRecording()
    {
        Debug.Log("recording started");
        StartCoroutine(RecordEyeTrackingData());
        recordingEnded = false;
    }

    public void StopRecording()
    {
        recordingEnded = true;
    }
    
    private IEnumerator RecordEyeTrackingData()
    {
        
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

            dataFrame.TimeStamp = 0; //TODO
            
            dataFrame.HMDposition = _hmdTransform.position; //to be impleneted

            dataFrame.NoseVector =  _hmdTransform.forward;


            _recordedEyeTrackingData.Add(dataFrame);
        }
        yield return new WaitForSeconds(_sampleRate);
    }
}
