using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VRCam : MonoBehaviour
{
    public bool seatActivated;
    private GameObject _seatPosition;
    
    private Vector3 _formerPosition;

    // private GameObject _calibrationOffset;
    // [SerializeField] private GameObject _camera;


    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        

        // seatPosition = CameraManager.Instance.GetObjectToFollow().GetComponent<CarController>().GetSeatPosition();
        _formerPosition = new Vector3();
    }
    
    private void  OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _seatPosition = CameraManager.Instance.GetSeatPosition();
    }

    private void LateUpdate()
    {
        if (seatActivated)
        {
            _seatPosition = CameraManager.Instance.GetSeatPosition();
            /*if (seatPosition == null)
            {
                Debug.Log("<color=red>Error: </color>Seat position is not assigned!");
                return;
            }*/
            
            transform.SetPositionAndRotation(_seatPosition.transform.position,_seatPosition.transform.rotation);
        }
    }


    

    public void Seat()
    {
        seatActivated = true;
    }

    public void UnSeat()
    {
        transform.position = _formerPosition;
    }

    /*public GameObject GetCamera()
    {
        return _camera;
    }*/

    /*public GameObject GetCameraOffset()
    {
        return _calibrationOffset;
    }*/
    
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        _formerPosition = position;
        seatActivated = false;
    }

    /*public void SetSeatPosition(GameObject seat)
    {
        seatPosition = seat;
    }*/
    
    /*public void SetOffset(Vector3 localOffset)
    {
        _calibrationOffset.transform.localPosition = localOffset;
    }*/

}
