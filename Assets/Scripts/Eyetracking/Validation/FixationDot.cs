using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixationDot : MonoBehaviour
{
    public GameObject TargetPoint;

    private float fixationDuration;

    public float aimedFixationDuration;
    // Start is called before the first frame update
    void Start()
    {
        fixationDuration = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, 50.0f))
        {
            if (hit.collider.gameObject == TargetPoint)
            {
                TargetPoint.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
            }

            fixationDuration = 1f * Time.deltaTime;
        }
        else
        {
            TargetPoint.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }
    }
}
