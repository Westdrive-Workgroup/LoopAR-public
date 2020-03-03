using System;
using System.Collections.Generic;
using LoopAr.Connector;
using LoopAr.DataTransferObjects;
using LoopAr.Internals;
using UnityEngine;

namespace LoopAr.Controller
{
    public class ParticipantController : MonoBehaviour
    {
        //TODO: Here should be stored the camera of the participant and the tracking behaviour defined
        //maybe also the UI-Generation? or is this still a part of the Manager?
        private List<ParticipantInformationData> _informationData;
        private Camera participantCamera; //TODO: should be grabed from the Manager
        private List<HitPositionData> _hitInformationData;
        private Transform trackedTransform;

        public void Start()
        {
            trackedTransform = gameObject.transform;
        }

        public void PrepareDataForTracking()
        {
            if (_informationData == null)
                _informationData = new List<ParticipantInformationData>();
            if (_hitInformationData == null)
                _hitInformationData = new List<HitPositionData>();
        }

        public void TrackInformation()
        {
            //Maybe an _informationData == null check?
            var information = new ParticipantInformationData
            {
                ParticipantNumber = name,
                EventTime = Time.time.ToString(),
                EyeTrackingTimeStamp = EyeTrackingConnector.GetEyeTrackingTimeStamp(),
                PositionRotationInformationData = new PositionRotationInformationData
                {
                    FrameNumber = Time.frameCount,
                    PositionInformation = trackedTransform.position,
                    RotationInformation = trackedTransform.rotation
                },
            };
            LoopArRayCaster.GenerateRayCastObject(trackedTransform.position, trackedTransform.rotation,
                trackedTransform.forward, participantCamera, out var rayCastData, out var hitPositionData);
            information.RayCastData = rayCastData;
            
            _hitInformationData.Add(hitPositionData);
            _informationData.Add(information);
        }
    }
}