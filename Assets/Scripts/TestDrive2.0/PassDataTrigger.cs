using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassDataTrigger : MonoBehaviour
{
    [SerializeField] private TestEventManager testEventManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ManualController>())
        {
            testEventManager.PassDataTrigger();
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
