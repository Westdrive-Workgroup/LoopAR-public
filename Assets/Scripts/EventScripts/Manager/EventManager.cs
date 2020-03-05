using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class EventManager : MonoBehaviour
    {
        #region Singelton

        public static EventManager instance;
        
        private void Start()
        {
        
        }
        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        #endregion

//        [SerializeField] private List<EventController> _eventControllers;
//
//        private List<EventMetaData> _eventMetaDatas;
//
//        public void FinishCurrentEvent(EventController eventController)
//        {
//            _currentMetaData.Finish(eventController);
//            _eventMetaDatas.Add(_currentMetaData);
//        }
    }
}