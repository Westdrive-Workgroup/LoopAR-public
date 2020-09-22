using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using RoboRyanTron.Unite2017.Variables;
using UnityEngine;

public class SwitchControlTrigger : MonoBehaviour
{
    [SerializeField] private PathCreator newPath;
    [SerializeField] private TestEventManager testEventManager;
    [SerializeField] private float newSpeed;
    [SerializeField] private float delayTillEventDeactivates;
    
    [SerializeField] private FloatVariable maxTrials;
    [SerializeField] private FloatVariable trialsDone;
    [SerializeField] private FloatVariable timeToWait;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarController>())
        {
            // Debug.Log("AIDRIVE END!    ");
            TrainingHandler.Instance.GoToMainExperiment();
            StartCoroutine(GiveAIControl(other));
            StartCoroutine(testEventManager.DeactivateEvent(delayTillEventDeactivates));
            testEventManager.DeactivateHUD();
        }
    }

    private IEnumerator GiveAIControl(Collider other)
    {
        other.gameObject.GetComponent<CarController>().TurnOffEngine();
        other.gameObject.GetComponent<ControlSwitch>().SwitchControl(false);

        if (trialsDone.Value >= maxTrials.Value)
        {
            yield return new WaitForSecondsRealtime(timeToWait.Value);
        }
        else
        {
            yield return new WaitForEndOfFrame();
        }
        
        other.gameObject.GetComponent<AimedSpeed>().SetRuleSpeed(newSpeed/3.6f);
        other.gameObject.GetComponent<CarController>().TurnOnEngine();

        other.gameObject.GetComponent<AIController>().SetNewPath(newPath);
        other.gameObject.GetComponent<AIController>().SetLocalTargetAndCurveDetection();
        GetComponent<BoxCollider>().enabled = false;
    }
}
