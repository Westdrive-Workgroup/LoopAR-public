using System;
using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Variables;
using UnityEngine;

public class TurnScreenBlackTrigger : MonoBehaviour
{
    [SerializeField] private FloatVariable timeToWait;
    
    [SerializeField] private FloatVariable maxTrials;
    [SerializeField] private FloatVariable trialsDone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarController>())
        {
            StartCoroutine(TurnScreenBlack());
        }
    }

    private IEnumerator TurnScreenBlack()
    {
        if (trialsDone.Value <= maxTrials.Value)
        {
            CameraManager.Instance.FadeOut();
            yield return new WaitForSecondsRealtime(timeToWait.Value);
            CameraManager.Instance.FadeIn();
        }

        else
        {
            CameraManager.Instance.FadeOut();
            yield return new WaitForSecondsRealtime(1);
            CameraManager.Instance.FadeIn();
        }
    }
}
