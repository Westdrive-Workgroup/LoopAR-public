using System;
using System.Collections;
using System.Collections.Generic;
using ForceFeedback;
using UnityEditor;
using UnityEngine;

[InitializeOnLoadAttribute]
public class SteeringWheelModelController : MonoBehaviour
{
    [SerializeField] private GameObject SteeringWheelModel;
    [SerializeField] private CarController carController;

    private float xRot; //this value is important for 
    // Start is called before the first frame update
    private void Start()
    {
        xRot = gameObject.transform.rotation.eulerAngles.x;
        
    }
    // Update is called once per frame
    
    //1.5 turns are 180 + 360 degrees , therefore 540 
    private void LateUpdate()
    {
        carController.GetSterring();
        Debug.Log(carController.GetSterring());
        this.transform.localRotation = Quaternion.Euler(xRot,0,carController.GetSterring()*540);
    }
    
    
    

    
 
}
