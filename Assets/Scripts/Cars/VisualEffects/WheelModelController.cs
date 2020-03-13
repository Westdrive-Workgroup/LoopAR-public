using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelModelController : MonoBehaviour
{
    [SerializeField]private GameObject wheelModel;
    private WheelCollider wheelCol;
    
    public GameObject GetModel()
    {
        return wheelModel;
    }

    private void Start()
    {
        wheelCol = GetComponent<WheelCollider>();
    }
    // Start is called before the first frame update


    public void SyncronizeModel(Vector3 pos, Quaternion rot)
    {
        wheelModel.transform.position = pos;
        wheelModel.transform.rotation = rot;
    }

    private void LateUpdate()
    {
        Vector3 pos;
        Quaternion rot;
        wheelCol.GetWorldPose(out pos, out  rot);
        SyncronizeModel(pos,rot);
    }
}
