using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictedZoneTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ManualController>() != null)
        {
            Debug.Log("<color=orange>"+ this.gameObject + " You have dieded "+ "</color>");
        }
    }
}
