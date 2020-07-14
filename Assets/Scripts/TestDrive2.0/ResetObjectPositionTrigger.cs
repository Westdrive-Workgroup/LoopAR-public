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

    [SerializeField] private float timeNoControl;

    

    private void Start()
    {
        _resetPosition = respawnPointObstacleHit.transform;
    }

    private void ResetCar(GameObject objectToReset)
    {
        objectToReset.transform.SetPositionAndRotation(_resetPosition.position, _resetPosition.rotation);

        objectToReset.GetComponent<Rigidbody>().velocity = Vector3.zero;
        objectToReset.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        objectToReset.transform.SetPositionAndRotation(_resetPosition.position, _resetPosition.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CarController>())
        {
            if (trialsDone.Value >= maxTrials.Value)
            {
                _resetPosition = respawnPointTrialFailed.transform;
            }
            ResetCar(other.gameObject);
            trialsDone.ApplyChange(1);

            StartCoroutine(TakeAwayControl(other));
        }
    }

    private IEnumerator TakeAwayControl(Collider other)
    {
        if (trialsDone.Value <= maxTrials.Value)
        {
            other.gameObject.SetActive(false);

            yield return new WaitForSecondsRealtime(timeNoControl);

            other.gameObject.SetActive(true);
        }

    }
}
