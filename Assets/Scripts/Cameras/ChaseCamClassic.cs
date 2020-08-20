using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ChaseCamClassic : MonoBehaviour
{
    [SerializeField] private GameObject _objectToFollow;
    [Range(0f, 10f)] public float damping;
    
    
    
    private void LateUpdate()
    {
        if (_objectToFollow == null)
        {
            return;
        }
        this.transform.position = _objectToFollow.transform.position;
        
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, _objectToFollow.transform.rotation,
            Time.deltaTime * damping);
    }
}