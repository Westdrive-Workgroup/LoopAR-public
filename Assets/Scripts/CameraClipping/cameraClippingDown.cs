using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraClippingDown: MonoBehaviour
{
    [SerializeField] private float interval;
    [SerializeField] private float maxRange;
    // Start is called before the first frame update

  
    void OnTriggerEnter(Collider other)
    {
        StartCoroutine("ClippingUp");
    }

    IEnumerator ClippingUp()
    {
        Camera cam = Camera.main;
        ;
        for (float range = cam.farClipPlane; range >= maxRange; range -= interval)
        {
            cam.farClipPlane = range;
            yield return new WaitForSeconds(0.1f);
        }
    }
    
}

