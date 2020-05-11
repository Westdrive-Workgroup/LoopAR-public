using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    [SerializeField] private TestEventManager testEventManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ManualController>())
        {
            GetComponent<BoxCollider>().enabled = false;
            testEventManager.EndTrigger(other);
        }
    }
}
