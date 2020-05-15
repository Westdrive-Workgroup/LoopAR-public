using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    [SerializeField] private int aimedSpeed;
    [SerializeField] private TestEventManager testEventManager;

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarController>())
        {
            other.gameObject.GetComponent<ManualController>().enabled = false;
            yield return new WaitForSeconds(5);
            other.gameObject.GetComponent<ManualController>().enabled = true;
            other.gameObject.GetComponent<AimedSpeed>().SetRuleSpeed(aimedSpeed);
            other.gameObject.GetComponent<ControlSwitch>().SwitchControl(false);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
