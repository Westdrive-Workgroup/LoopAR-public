using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(CarControl))]
public class GearSystem : MonoBehaviour
{
   private CarControl _carController;

   private float _currentSpeedinKmH;

   private int currentGear = 1;
   private float _currentTorque;
   private float _maximalTorque;

   private void Start()
   {
      _carController = GetComponent<CarControl>();
      _maximalTorque = _carController.GetTorque();       //The Original Torque is the maximal the Car is able to put
   }

   private void FixedUpdate()
   {
      _currentSpeedinKmH = _carController.GetCurrentSpeedInKmH();
      
      //_carController.SetTorque(SetTorqueBySpeed(_currentSpeedinKmH));
   }

   private void Update()
   {
      
      
      _carController.SetTorque(SetTorqueBySpeed(_currentSpeedinKmH));
   }

   private float SetTorqueBySpeed(float speed)
   {
      if (_currentSpeedinKmH <10)
      {
         _currentTorque = _maximalTorque;
         currentGear = 1;
      }else if (_currentSpeedinKmH > 10f && _currentSpeedinKmH < 20f)
      {
         _currentTorque = _maximalTorque * 0.9f;
         currentGear = 2;
      }else if (_currentSpeedinKmH > 20f &&_currentSpeedinKmH < 50)
      {
         _currentTorque = _maximalTorque * 0.7f;
         currentGear = 3;
      }else if (_currentSpeedinKmH > 50 && _currentSpeedinKmH < 70)
      {
         _currentTorque = _maximalTorque * 0.4f;
         currentGear = 4;
      }else if (_currentSpeedinKmH < 120)
      {
         _currentTorque = _maximalTorque * 0.1f;
         currentGear = 5;
      }else if (_currentSpeedinKmH > 120)
      {
         _currentTorque = _maximalTorque * 0.05f;
         currentGear = 6;
      }

      return _currentTorque;
   }
}
