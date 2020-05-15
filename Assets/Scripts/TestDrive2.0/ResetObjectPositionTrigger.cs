using System;
using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Variables;
using UnityEngine;
using Object = System.Object;

public class ResetObjectPositionTrigger : MonoBehaviour
{

    [Tooltip("The point where the object will get respawned to.")]
    [SerializeField] private GameObject respawnPoint;
  
    [SerializeField] private FloatVariable maxTrials;
    [SerializeField] private FloatVariable trialsDone;
   
    private Transform resetPosition;

    

    private void Start()
    {
        resetPosition = respawnPoint.transform;
    }

    private void ResetCar(GameObject objectToReset)
    {
        if (objectToReset.GetComponent<CarController>())
        {
            objectToReset.GetComponent<Rigidbody>().velocity = Vector3.zero;
            objectToReset.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            objectToReset.transform.SetPositionAndRotation(resetPosition.position, resetPosition.rotation);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CarController>())
        {
            if (trialsDone.Value < maxTrials.Value)
            {
                trialsDone.ApplyChange(1);
            }
            else
            {
                CalibrationManager.Instance.TestDriveFailed();
            }
            ResetCar(other.gameObject);
        }
    }
}
