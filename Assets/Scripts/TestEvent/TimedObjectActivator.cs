using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class TimedObjectActivator : MonoBehaviour
{
    [Tooltip("Game Object to be displayed.")]
    public GameObject toBeDisplayed;
    
    [Tooltip("How many seconds till the object is deactivated again.")]
    public float delayTime;
    
    public void ActivateObject()
    {
        toBeDisplayed.SetActive(true);
    }

    private void OnEnable()
    {
        StartCoroutine(DeactivateThisObject(toBeDisplayed, delayTime));
    }
    
    IEnumerator DeactivateThisObject(GameObject toBeActivated, float delay)
    {
        yield return new WaitForSeconds(delay);
        toBeActivated.SetActive(false);
    }
}
