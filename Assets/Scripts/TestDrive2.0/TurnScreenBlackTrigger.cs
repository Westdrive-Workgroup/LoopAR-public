using System;
using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Variables;
using UnityEngine;

public class TurnScreenBlackTrigger : MonoBehaviour
{
    
    [SerializeField] private Canvas canvas;
    private CanvasGroup _canvasGroup;

    [SerializeField] private float timeToWait;
    
    [SerializeField] private FloatVariable maxTrials;
    [SerializeField] private FloatVariable trialsDone;


    private void Awake()
    {
        //_canvasGroup = canvas.GetComponent<CanvasGroup>();
    }

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
            //_canvasGroup.alpha = 1;
            yield return new WaitForSecondsRealtime(timeToWait);
            //_canvasGroup.alpha = 0;
            CameraManager.Instance.FadeIn();
        }
    }
}
