using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    [SerializeField] private TestEventManager testEventManager;
    [SerializeField] private float secondsTillManualControl;

    [SerializeField] private GameObject zoneLimiter;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarController>())
        {
            StartCoroutine(Triggered(other));
        }
    }

    private IEnumerator Triggered(Collider other)
    {
        StartCoroutine(testEventManager.ActivateHUD());
        
        // Debug.Log("Before waiting End...");
        yield return new WaitForSecondsRealtime(secondsTillManualControl);
        // Debug.Log("After waiting End...");
        
        GetComponent<BoxCollider>().enabled = false;
        zoneLimiter.gameObject.SetActive(true);
        
        testEventManager.EndTrigger(other);
        GetComponent<BoxCollider>().enabled = false;
        // Debug.Log("Does he make it here?");
        
    }
}
