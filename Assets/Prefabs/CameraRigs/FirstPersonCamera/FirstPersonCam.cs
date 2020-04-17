using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonCam : MonoBehaviour
{
    public GameObject seatPosition;

    public bool allowMouseRotation;
    [Range(0f, 700f)]public float MouseSensivity;
    public bool invertedMouse;
    
    private Vector3 localRotation;
    // Start is called before the first frame update
    void Start()
    {
    }

    
    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.position = seatPosition.transform.position;
        if(allowMouseRotation)
            PerformRotation();
    }


    void PerformRotation()
    {
        //Debug.Log("rotate" + " x : " + Input.GetAxis("Mouse X") + "y: "+ Input.GetAxis("Mouse Y"));
        float pitch_change = Input.GetAxis("Mouse X") * MouseSensivity * Time.deltaTime;
        float yaw_change = Input.GetAxis("Mouse Y") * MouseSensivity * Time.deltaTime;
        int invertionfactor = -1;

        if (invertedMouse)
        {
            invertionfactor = 1;
        }

        localRotation.y += pitch_change;
        localRotation.x += Mathf.Clamp(yaw_change * invertionfactor, -90, 90);
        localRotation.z = 0;
        //Debug.Log(localRotation);
        

        transform.rotation= Quaternion.Euler(localRotation);
    }
    
}
