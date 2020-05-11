using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    [SerializeField] private TestEventManager testEventManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ManualController>())
        {
            testEventManager.StartTrigger(other);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
