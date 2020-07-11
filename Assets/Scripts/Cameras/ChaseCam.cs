using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ChaseCam : MonoBehaviour
{
    public GameObject objectToFollow;
    [Range(0f, 10f)] public float damping;
    
    private void LateUpdate()
    {
        if (objectToFollow == null)
        {
            Debug.LogWarning("Object to follow not found!");
            return;
        }
        
        this.transform.position = objectToFollow.transform.position;
        
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, objectToFollow.transform.rotation,
            Time.deltaTime * damping);
    }
}