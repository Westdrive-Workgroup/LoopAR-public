using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class CameraManager : MonoBehaviour
{
    #region Fields

    public static CameraManager Instance { get; private set; }
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject calibrationOffset;
    [SerializeField] private GameObject objectToFollow;
    
    private GameObject _seatPosition;
    private VRCam _vRCamera;
    private bool _vRState;

    #endregion

    #region PrivateMethods

    private void Awake()
    {
        //singleton pattern a la Unity
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /*private void Start()
    {
        if (CalibrationManager.Instance.GetVRActivationState())
        {
            this.gameObject.GetComponent<ChaseCam>().enabled = false;
            _vRCamera = this.gameObject.GetComponent<VRCam>();
            mainCamera.GetComponent<Camera>().stereoTargetEye = StereoTargetEyeMask.Both;
            VRModeCamera();
        }
        else
        {
            this.gameObject.GetComponent<VRCam>().enabled = false;
            mainCamera.GetComponent<Camera>().stereoTargetEye = StereoTargetEyeMask.None;
            NonVRModeCamera();
        }

        _vRState = CalibrationManager.Instance.GetVRActivationState();
    }*/

    public void VRModeCamera()
    {
        this.gameObject.GetComponent<ChaseCam>().enabled = false;
        _vRCamera = this.gameObject.GetComponent<VRCam>();
        mainCamera.GetComponent<Camera>().stereoTargetEye = StereoTargetEyeMask.Both;
        
        FadeIn();
        
        if (CalibrationManager.Instance != null)
        {
            SetOffset(CalibrationManager.Instance.GetSeatCalibrationOffset());
        }
        else
        {
            // calibrationOffset.transform.localPosition = Vector3.zero;
            SetOffset(Vector3.zero);
            Debug.Log("<color=red>Error: </color>No Calibration Manager found, please add to the scene.");
        }
        
        if (objectToFollow != null)
        {
            this.gameObject.transform.position = GetSeatPositionVector3();
            _vRCamera.SetPosition(GetSeatPositionVector3()); ///////?????
        }
        
        _vRState = CalibrationManager.Instance.GetVRActivationState();
    }

    public void NonVRModeCamera()
    {
        this.gameObject.GetComponent<VRCam>().enabled = false;
        mainCamera.GetComponent<Camera>().stereoTargetEye = StereoTargetEyeMask.None;
        
        blackScreen.SetActive(true);
        if (objectToFollow != null)
        {
            // this.gameObject.transform.position = objectToFollow.transform.position;
            SetOffset(Vector3.zero);
            this.transform.position = GetSeatPositionVector3();
        }
        
        _vRState = CalibrationManager.Instance.GetVRActivationState();
    }

    #endregion

    #region PublicMethods

    public void FadeOut()
    {
        if (_vRState)
        {
            //set start color
            SteamVR_Fade.Start(Color.clear, 0f);
            //set and start fade to
            SteamVR_Fade.Start(Color.black, 0);
        }
        else
        {
            blackScreen.SetActive(true);
        }
    }

    public void FadeIn()
    {
        if (_vRState)
        {
            //set start color
            SteamVR_Fade.Start(Color.black, 0f);
            //set and start fade to
            SteamVR_Fade.Start(Color.clear, 0f);
        }
        else
        {
            blackScreen.SetActive(false);
        }
    }

        #region Setters

        public void SetObjectToFollow(GameObject theObject)
        {
            objectToFollow = theObject;
        }

        public void SetSeatPosition(GameObject seat)
        {
            _seatPosition = seat;
        }
        
        public void SetOffset(Vector3 localOffset)
        {
            calibrationOffset.transform.localPosition = localOffset;
        }

        #endregion
    

        #region Getters

        public GameObject GetObjectToFollow()
        {
            return objectToFollow;
        }

        public VRCam GetVRCamera()
        {
            return _vRCamera;
        }

        public GameObject GetSeatPosition()
        {
            return _seatPosition;
        }
        
        public Vector3 GetSeatPositionVector3()
        {
            return objectToFollow.GetComponent<CarController>().GetSeatPosition().transform.position;
        }

        public GameObject GetCalibrationOffset()
        {
            return calibrationOffset;
        }

        public Camera GetMainCamera()
        {
            return mainCamera;
        }

        #endregion
    
    
    #endregion
}
