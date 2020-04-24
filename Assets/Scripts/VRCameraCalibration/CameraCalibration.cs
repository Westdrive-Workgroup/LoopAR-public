using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

public class CameraCalibration : MonoBehaviour
{
    private string dataPath;
    private Vector3 seatPosition;
    private GameObject _camera;
    private GameObject _cameraArm;
    

    private Vector3 distanceVector;

    private String calibrationFilePath;

    public GameObject calibrationOffset;
    // Start is called before the first frame update
    private void Awake()
    {
        calibrationFilePath = GetPathForSaveFile("CalibrationData");
        
        _camera = GetComponent<VRCam>().GetCamera();
        distanceVector = LoadCalibrationFile(calibrationFilePath);

        calibrationOffset.transform.localPosition += distanceVector;
        _camera.transform.SetParent(calibrationOffset.transform);
        _camera.transform.localPosition = Vector3.zero;
        Debug.Log( "camera is positioned with " + calibrationOffset );
    }

    private void Start()
    {
        _cameraArm = this.transform.gameObject;
        
        

        //distanceVector = LoadCalibrationFile(calibrationFilePath);
        _camera.transform.localPosition = distanceVector;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            GetComponent<VRCam>().Seat();
            
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            distanceVector.x = _cameraArm.transform.position.x - _camera.transform.position.x;
            distanceVector.y = _cameraArm.transform.position.y - _camera.transform.position.y;
            distanceVector.z = _cameraArm.transform.position.z - _camera.transform.position.z;
            
            Debug.Log(distanceVector);
            
            SaveCalibrationFile(distanceVector);
            //PlayerPrefs.SetFloat("_hmd_offset_x", distanceVector.x);
            //PlayerPrefs.SetFloat("_hmd_offset_y", distanceVector.y);
            //PlayerPrefs.SetFloat("_hmd_offset_y", distanceVector.z);
            
            //PlayerPrefs.Save();

            //Debug.Log(PlayerPrefs.GetFloat("_hmd_offset_x"));
            //Debug.Log("real " + distanceVector.x);
            //Debug.Log(PlayerPrefs.Get("_hmd_offset_y"));
            //Debug.Log("real " + distanceVector.y);
            //Debug.Log(PlayerPrefs.GetFloat("_hmd_offset_z"));
            //_camera.transform.Translate(Vector3.down * 2f);
            //Valve.VR.OpenVR.System.GetSeatedZeroPoseToStandingAbsoluteTrackingPose()

        }
    }

    Vector3 LoadCalibrationFile(string dataPath)
    {
        string jsonString;
        if(!File.Exists(dataPath))
        {
            Debug.Log("file not found");
            return Vector3.zero;
        }
        else
        {
            Debug.Log("found data");
            jsonString = File.ReadAllText(dataPath);
            //Debug.Log(jsonString);
            return JsonUtility.FromJson<Vector3>(jsonString);
        }
            
          
        
        

        
        
    }
    
    void SaveCalibrationFile(Vector3 calibrationData)
    {

        string jsonString = JsonUtility.ToJson(calibrationData);
   
        File.WriteAllText(calibrationFilePath, jsonString);
        
        
    }
    
    private string GetPathForSaveFile(string saveFileName)
    {
        return Path.Combine(Application.persistentDataPath, saveFileName + ".txt");
    }
}
