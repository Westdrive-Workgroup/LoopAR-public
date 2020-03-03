using System;
using System.Collections.Generic;
using LoopAr.DataTransferObjects;
using UnityEngine;

namespace LoopAr.Controller
{
    public abstract class AbstractMovableObjectController : MonoBehaviour
    {
        protected string nameOfMovableObject;
        protected GameObject trackedObject;
        protected List<PositionRotationInformationData> trackedData;

        public void Start()
        {
            trackedObject = gameObject;
            trackedData = new List<PositionRotationInformationData>();
        }

        public void TrackData()
        {
            trackedData.Add(new PositionRotationInformationData
                {
                    FrameNumber = Time.frameCount,
                    PositionInformation = trackedObject.transform.position,
                    RotationInformation = trackedObject.transform.rotation
                }
            );
        }
    }
}