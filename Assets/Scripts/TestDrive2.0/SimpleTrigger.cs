using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ManualController>())
        {
            Debug.Log("SimpleTrigger");
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
