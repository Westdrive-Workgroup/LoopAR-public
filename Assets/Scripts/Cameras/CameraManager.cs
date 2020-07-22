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
    private GameObject _objectToFollow;
    
    private GameObject _seatPosition;
    private VRCam _vRCamera;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ReSpawnBehavior();
    }

    private void Start()
    {
        if (CalibrationManager.Instance.GetVRActivationState())
        {
            VRModeCamera();
        }
        else
        {
            NonVRModeCamera();
        }
    }

    #endregion

    #region PublicMethods

    public void VRModeCamera()
    {
        this.gameObject.GetComponent<VRCam>().enabled = true;
        this.gameObject.GetComponent<ChaseCam>().enabled = false;
        _vRCamera = this.gameObject.GetComponent<VRCam>();
        mainCamera.GetComponent<Camera>().stereoTargetEye = StereoTargetEyeMask.Both;

        // FadeOut();
        
        if (CalibrationManager.Instance != null)
        {
            SetOffset(CalibrationManager.Instance.GetSeatCalibrationOffset());
        }
        else
        {
            SetOffset(Vector3.zero);
            Debug.Log("<color=red>Error: </color>No Calibration Manager found, please add to the scene.");
        }
        
        if (_objectToFollow != null)
        {
            this.gameObject.transform.position = GetSeatPositionVector3();
            _vRCamera.SetPosition(GetSeatPositionVector3()); ///////????? todo
        }
    }

    public void NonVRModeCamera()
    {
        this.gameObject.GetComponent<ChaseCam>().enabled = true;
        this.gameObject.GetComponent<VRCam>().enabled = false;
        mainCamera.GetComponent<Camera>().stereoTargetEye = StereoTargetEyeMask.None;
        mainCamera.GetComponent<Camera>().fieldOfView = 60f;
        
        // blackScreen.SetActive(true);
        if (_objectToFollow != null)
        {
            SetOffset(Vector3.zero);
            this.transform.position = GetSeatPositionVector3();
        }
    }
    
    public void FadeOut()
    {
        if (CalibrationManager.Instance.GetVRActivationState())
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
        if (CalibrationManager.Instance.GetVRActivationState())
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
    
    public void AlphaFadeIn()
    {
        _objectToFollow.GetComponent<CarWindows>().SetInsideWindowsAlphaChannel(0);
    }
    
    public void AlphaFadeOut()
    {
        _objectToFollow.GetComponent<CarWindows>().SetInsideWindowsAlphaChannel(1);
    }

        #region Setters

        public void SetObjectToFollow(GameObject theObject)
        {
            _objectToFollow = theObject;
        }

        public void SetSeatPosition(GameObject seat)
        {
            _seatPosition = seat;
        }
        
        public void SetOffset(Vector3 localOffset)
        {
            calibrationOffset.transform.localPosition = localOffset;
        }

        public void ReSpawnBehavior()
        {
            if (CalibrationManager.Instance.GetVRActivationState())
            {
                // todo
            }
            else
            {
                this.gameObject.GetComponent<ChaseCam>().ForceChaseCamRotation();
            }
        }

        #endregion
    

        #region Getters
        
        private Vector3 GetSeatPositionVector3()
        {
            return _objectToFollow.GetComponent<CarController>().GetSeatPosition().transform.position;
        }

        public GameObject GetObjectToFollow()
        {
            return _objectToFollow;
        }

        public VRCam GetVRCamera()
        {
            return _vRCamera;
        }

        public GameObject GetSeatPosition()
        {
            return _seatPosition;
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
