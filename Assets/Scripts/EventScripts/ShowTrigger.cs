using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTrigger : MonoBehaviour
{
   [SerializeField] private Color color = Color.magenta;
   
   private void OnDrawGizmos()
   {
      Gizmos.color = color;
      Gizmos.DrawCube(transform.position, GetComponent<Collider>().bounds.size);
   }
}
