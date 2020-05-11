using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class ResetObjectPositionTrigger : MonoBehaviour
{

    [Tooltip("The point where the object will get respawned to.")]
    [SerializeField] private GameObject respawnPoint;
    private Transform resetPosition;

    private void Start()
    {
        resetPosition = respawnPoint.transform;
    }

    private void ResetCar(GameObject ObjectToReset)
    {
        if (ObjectToReset.GetComponent<ManualController>())
        {
            ObjectToReset.transform.SetPositionAndRotation(resetPosition.position, resetPosition.rotation);
            ObjectToReset.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        ResetCar(other.gameObject);
    }
}
