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

        FadeOut();
        
        if (CalibrationManager.Instance != null)
        {
            SetOffset(CalibrationManager.Instance.GetSeatCalibrationOffset());
        }
        else
        {
            SetOffset(Vector3.zero);
            Debug.Log("<color=red>Error: </color>No Calibration Manager found, please add to the scene.");
        }
        
        if (objectToFollow != null)
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
        
        blackScreen.SetActive(true);
        if (objectToFollow != null)
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
        /*// Alpha start value.
        float alpha = 1.0f;
 
        // Loop until aplha is below zero (completely invisalbe)
        while (alpha > 0.0f)
        {
            // Reduce alpha by fadeSpeed amount.
            alpha -= fadeSpeed * Time.deltaTime;
            Debug.Log("Fade in! alpha: " + alpha);
            
            objectToFollow.GetComponent<CarWindows>().SetInsideWindowsAlphaChannel(alpha);
        }*/
        
        objectToFollow.GetComponent<CarWindows>().SetInsideWindowsAlphaChannel(0);
    }
    
    public void AlphaFadeOut()
    {
        /*// Alpha start value.
        float alpha = 0.0f;
 
        // Loop until aplha is below zero (completely invisalbe)
        while (alpha < 1.0f)
        {
            // Reduce alpha by fadeSpeed amount.
            alpha += fadeSpeed * Time.deltaTime;
            Debug.Log("Fade out! alpha: " + alpha);
            
            objectToFollow.GetComponent<CarWindows>().SetInsideWindowsAlphaChannel(alpha);
        }*/
        
        objectToFollow.GetComponent<CarWindows>().SetInsideWindowsAlphaChannel(1);
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
        
        private Vector3 GetSeatPositionVector3()
        {
            return objectToFollow.GetComponent<CarController>().GetSeatPosition().transform.position;
        }

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
