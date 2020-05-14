using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    [SerializeField] private TestEventManager testEventManager;
    [SerializeField] private int secondsTillManualControl;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarController>())
        {
            
          //other.gameObject.GetComponent<ManualController>().enabled = false;
          //testEventManager.EndTrigger(other);
          //Debug.Log("Before waiting...");
          //yield return new WaitForSeconds(secondsTillManualControl);
          //Debug.Log("After waiting...");
          //other.gameObject.GetComponent<ManualController>().enabled = true;
          //GetComponent<BoxCollider>().enabled = false;
          StartCoroutine(Triggered(other));

        }
    }

    IEnumerator Triggered(Collider other)
    {
        other.gameObject.GetComponent<ManualController>().enabled = false; 
        testEventManager.EndTrigger(other); 
        Debug.Log("Before waiting...");
        yield return new WaitForSeconds(secondsTillManualControl);
        Debug.Log("After waiting...");
        
        // If you remove this following line, it works
        testEventManager.ActivateHUD();
        other.gameObject.GetComponent<ManualController>().enabled = true;
        GetComponent<BoxCollider>().enabled = false;
    }
}
