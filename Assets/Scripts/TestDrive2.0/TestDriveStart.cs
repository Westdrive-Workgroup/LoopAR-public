using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDriveStart : MonoBehaviour
{
    [SerializeField] private GameObject carBody;

    [SerializeField] private int delay;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(PassControl());
        }
    }

    IEnumerator PassControl()
    {
        carBody.GetComponent<ControlSwitch>().SwitchControl(true);
        yield return new WaitForSeconds(delay);
        carBody.GetComponent<ControlSwitch>().SwitchControl(false);
    }

}
