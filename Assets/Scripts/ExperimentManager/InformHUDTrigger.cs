using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class InformHUDTrigger : MonoBehaviour
{
    //todo make an instance of HUD
    
    private GameObject _currentTarget;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _currentTarget)
            return;

        _currentTarget = other.gameObject;
        
        if (other.GetComponent<ManualController>() != null)
        {
            Debug.Log("<color=orange>Inform the HUD that car will switch!</color>");
            //todo call HUD    
        }
    }
    
    
}
