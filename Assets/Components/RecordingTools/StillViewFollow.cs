using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StillViewFollow : MonoBehaviour
{
    public GameObject followObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    void Update()
    {
        Vector3 lookOnObject = followObject.transform.position - transform.position;
        lookOnObject = followObject.transform.position - this.transform.position;
        this.transform.forward = lookOnObject.normalized;
    }
}
