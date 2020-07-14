using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class SwitchControlTrigger : MonoBehaviour
{
    [SerializeField] private PathCreator newPath;
    [SerializeField] private TestEventManager testEventManager;
    [SerializeField] private float newSpeed;
    [SerializeField] private float delayTillEventDeactivates;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarController>())
        {
            testEventManager.DeactivateHUD();
            StartCoroutine(testEventManager.DeactivateEvent(delayTillEventDeactivates));
            GiveAIControl(other);
        }
    }

    private void GiveAIControl(Collider other)
    {
        other.gameObject.GetComponent<AIController>().SetNewPath(newPath);
        other.gameObject.GetComponent<AIController>().SetLocalTargetAndCurveDetection();
        other.gameObject.GetComponent<AimedSpeed>().SetRuleSpeed(newSpeed/3.6f);
        other.gameObject.GetComponent<ControlSwitch>().SwitchControl(false);
        GetComponent<BoxCollider>().enabled = false;
    }
}
