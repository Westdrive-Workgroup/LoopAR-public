using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisStabilizer : MonoBehaviour
{
   public float antiRoll = 5000f;
   
   private CarController _carController;
   private WheelCollider frontLeft;
   private WheelCollider frontRight;
   private WheelCollider backLeft;
   private WheelCollider backRight;
   private WheelCollider[] frontAxis;
   private WheelCollider[] backAxis;
   private Rigidbody rb;

   private void Start()
   {
      rb = this.gameObject.GetComponent<Rigidbody>();
   }

   public void  AssignWheels(WheelCollider FrontLeft, WheelCollider FrontRight, WheelCollider BackLeft, WheelCollider BackRight)
   {
      frontLeft = FrontLeft;
      frontRight = FrontRight;
      backLeft = BackLeft;
      backRight= BackRight;
   }
    

   private void GroundWheels(WheelCollider left, WheelCollider right)
   {
      //calculate the proportion of distance and make sure the hit point is along the suspension
      
      //the wheel that has the greater distance to the ground is further sleight to its suspension rod
      WheelHit hit;
      float travelL = 1.0f; //how much the wheel has traveled
      float travelR = 1.0f;

      bool groundedLeft = left.GetGroundHit(out hit);
      if (groundedLeft)
         travelL = (-left.transform.InverseTransformPoint(hit.point).y -
                    left.radius) / left.suspensionDistance;
      
      bool groundedRight = right.GetGroundHit(out hit);
      if (groundedRight)
         travelR = (-right.transform.InverseTransformPoint(hit.point).y -
                    right.radius) / right.suspensionDistance;
      
      
      float antiRollForce= (travelL -travelR) *antiRoll;
      
      if(groundedLeft)
         rb.AddForceAtPosition(left.transform.up * -antiRollForce, left.transform.position);
      
      if(groundedRight)
         rb.AddForceAtPosition(right.transform.up * antiRollForce, right.transform.position);   
   }

   private void FixedUpdate()
   {
      GroundWheels(frontLeft, frontRight);
      GroundWheels(backLeft, backRight);
   }
}
