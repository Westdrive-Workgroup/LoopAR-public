using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchControlTrigger : MonoBehaviour
{
    [SerializeField] private bool manualDriving;
    private void OnTriggerEnter(Collider other)
    {
        
        other.GetComponent<ControlSwitch>().SwitchControl(manualDriving);

        Debug.Log("Manual Control active is " + manualDriving);
    }
}
