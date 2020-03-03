using System.Collections.Generic;
using LoopAr.DataTransferObjects;

namespace LoopAr.DataLayer.Mapper.ParticipantInformationDataMapper
{
    public abstract class
        ParticipantInformationAbstractMapper<TFormat> : GenericAbstractMapper<ParticipantInformationData, TFormat>
    {
        protected readonly Dictionary<string, int> PositionValueMap;

        protected const string ParticipantNumber = "ParticipantNumber";
        protected const string EventTime = "EventTime";
        protected const string EyeTrackingTimeStamp = "EyeTrackingTimeStamp";

        protected const string PositionInformationFrameNumber = "PositionInformationFrameNumber"; 
        protected const string PositionInformationX = "PositionX"; //Vector3
        protected const string PositionInformationY = "PositionY"; //Vector3
        protected const string PositionInformationZ = "PositionZ"; //Vector3

        protected const string RotationInformationX = "RotationX"; //Quaternion
        protected const string RotationInformationY = "RotationY"; //Quaternion
        protected const string RotationInformationZ = "RotationZ"; //Quaternion
        protected const string RotationInformationW = "RotationW"; //Quaternion

        //Center Object
        protected const string CenterHit = "CenterHit";
        protected const string CenterHitPositionX = "CenterHitPositionX"; //Vector3
        protected const string CenterHitPositionY = "CenterHitPositionY"; //Vector3
        protected const string CenterHitPositionZ = "CenterHitPositionZ"; //Vector3
        protected const string CenterHitGroup = "CenterHitGroup";

        //Box Object
        protected const string BoxHit = "BoxHit";
        protected const string BoxHitPositionX = "BoxHitPositionX"; //Vector3
        protected const string BoxHitPositionY = "BoxHitPositionY"; //Vector3
        protected const string BoxHitPositionZ = "BoxHitPositionZ"; //Vector3
        protected const string BoxHitGroup = "BoxHitGroup";

        //PresentedObject
        protected const string PresentedObjectName = "PresentedObjectName";
        protected const string PresentedObjectGroup = "PresentedObjectGroup";

        protected ParticipantInformationAbstractMapper()
        {
            PositionValueMap = new Dictionary<string, int>
            {
                {ParticipantNumber, 0},
                {EventTime, 1},
                {EyeTrackingTimeStamp, 2},

                {PositionInformationFrameNumber, 3},
                {PositionInformationX, 4},
                {PositionInformationY, 5},
                {PositionInformationZ, 6},

                {RotationInformationX, 7},
                {RotationInformationY, 8},
                {RotationInformationZ, 9},
                {RotationInformationW, 10},

                //Center Object
                {CenterHit, 11},
                {CenterHitPositionX, 12},
                {CenterHitPositionY, 13},
                {CenterHitPositionZ, 14},
                {CenterHitGroup, 15},

                //Box Object
                {BoxHit, 16},
                {BoxHitPositionX, 17},
                {BoxHitPositionY, 18},
                {BoxHitPositionZ, 19},
                {BoxHitGroup, 20},

                //PresentedObject
                {PresentedObjectName, 21},
                {PresentedObjectGroup, 22}
            };
        }
    }
}