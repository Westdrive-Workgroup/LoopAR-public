using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VRCam : MonoBehaviour
{
    #region Fields

    private bool _seatActivated;
    private GameObject _seatPosition;
    private Vector3 _formerPosition;

    #endregion
    
    #region PrivateMethods

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        if (CameraManager.Instance.GetSeatPosition() != null)
        {
            _seatPosition = CameraManager.Instance.GetSeatPosition();
        }
        
        _formerPosition = new Vector3();
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        /*if (CameraManager.Instance.GetSeatPosition() != null)
        {
            _seatPosition = CameraManager.Instance.GetSeatPosition();
        }*/
    }

    private void LateUpdate()
    {
        if (_seatActivated)
        {
            if (_seatPosition != null)
            {
                transform.SetPositionAndRotation(_seatPosition.transform.position, _seatPosition.transform.rotation);
            }
            else
            {
                Debug.Log("<color=red>Error: </color>Seat position is not assigned!");
            }
        }
    }

    #endregion

    #region PublicMethods

    public void Seat()
    {
        _seatActivated = true;
    }

    public void UnSeat()
    {
        transform.position = _formerPosition;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        _formerPosition = position;
        _seatActivated = false;
    }

    public void SetSeatPosition(GameObject seatPosition)
    {
        _seatPosition = seatPosition;
    }

    #endregion
}
