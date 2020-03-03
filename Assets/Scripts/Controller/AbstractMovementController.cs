using System;
using UnityEngine;

namespace LoopAr.Controller
{
    //TODO: Delete this as an Example Controller
    public abstract class AbstractMovementController : MonoBehaviour
    {
        [SerializeField] internal Color changeColor;
        [SerializeField] internal int colorChangeSeconds;
        internal Material _objectMaterial;
        internal Color _originColor;


        internal void Awake()
        {
            _objectMaterial = GetComponent<Renderer>().material;
            _originColor = _objectMaterial.color;
        }

        internal abstract void SetToPosition(Vector3 updatePosition);

        internal abstract void ShowMovementBehavior();
        
        
        public abstract void RequestColorChange();
        
        
    }
}