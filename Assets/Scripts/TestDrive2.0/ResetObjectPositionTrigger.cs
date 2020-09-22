using System;
using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Variables;
using UnityEngine;
using Object = System.Object;

public class ResetObjectPositionTrigger : MonoBehaviour
{

    [Tooltip("The point where the object will get respawned to after hitting an obstacle.")]
    [SerializeField] private GameObject respawnPointObstacleHit;
    
    [Tooltip("The point where the object will get respawned to exceeding the allowed number of trials.")]
    [SerializeField] private GameObject respawnPointTrialFailed;
  
    [SerializeField] private FloatVariable maxTrials;
    [SerializeField] private FloatVariable trialsDone;
   
    private Transform _resetPosition;

    [SerializeField] private FloatVariable timeToWait;

    

    private void Start()
    {
        _resetPosition = respawnPointObstacleHit.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CarController>())
        {
            if (trialsDone.Value >= maxTrials.Value)
            {
                TrainingHandler.Instance.GoToMainExperiment();
                _resetPosition = respawnPointTrialFailed.transform;
            }
            
            ResetCar(other.gameObject);
            trialsDone.ApplyChange(1);

            StartCoroutine(TakeAwayControl(other));
        }
    }
    
    private void ResetCar(GameObject objectToReset)
    {
        // objectToReset.SetActive(false);
        
        // objectToReset.transform.SetPositionAndRotation(_resetPosition.position, _resetPosition.rotation);

        objectToReset.GetComponent<CarController>().TurnOffEngine();
        objectToReset.GetComponent<Rigidbody>().isKinematic = true;
        objectToReset.GetComponent<Rigidbody>().velocity = Vector3.zero;
        // objectToReset.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        objectToReset.transform.SetPositionAndRotation(_resetPosition.position, _resetPosition.rotation);
    }

    private IEnumerator TakeAwayControl(Collider other)
    {
        
        yield return new WaitForEndOfFrame();
        
        yield return new WaitForSecondsRealtime(timeToWait.Value);

        // other.gameObject.SetActive(true);
        other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        other.gameObject.GetComponent<CarController>().TurnOnEngine();
        
        //if (trialsDone.Value <= maxTrials.Value)
        //{
        //    //other.gameObject.SetActive(false);
        //
        //    
        //}

    }
}
