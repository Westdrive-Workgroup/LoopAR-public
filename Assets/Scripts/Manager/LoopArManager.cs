using System;
using System.Collections.Generic;
using UnityEngine;

namespace LoopAr.Controller
{
    public class LoopArManager : MonoBehaviour
    {

        #region Singelton

        public static LoopArManager instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            DontDestroyOnLoad(this);
        }

        #endregion
        

        [SerializeField] private List<AbstractMovementController> _movementControllers;
        
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                foreach (AbstractMovementController abstractMovementController in _movementControllers)
                {
                    abstractMovementController.RequestColorChange();
                }
            }
        }
    }
}