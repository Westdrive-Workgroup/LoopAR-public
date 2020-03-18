using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Matrix4x4 = System.Numerics.Matrix4x4;

public class RestrictedZoneTrigger : MonoBehaviour
{
    private Vector3 _colliderdimensions;

    private Vector3 _colliderPosition;
    // Start is called before the first frame update
    void Start()
    {
        _colliderdimensions = GetComponent<Collider>().bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ManualController>() != null)
        {
            Debug.Log("<color=orange>"+ this.gameObject + " You have dieded "+ "</color>");
        }
    }

    public void OnDrawGizmos()
    {
        //Gizmos.color=Color.red;
        //Gizmos.matrix= transform.localToWorldMatrix;
        
        //Gizmos.DrawWireCube(this.transform.position,_colliderdimensions);
    }
}
