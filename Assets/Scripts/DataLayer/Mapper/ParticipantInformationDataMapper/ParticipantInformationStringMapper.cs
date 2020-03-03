using System.Collections.Generic;
using LoopAr.DataTransferObjects;

namespace LoopAr.DataLayer.Mapper.ParticipantInformationDataMapper
{
    public class ParticipantInformationStringMapper : ParticipantInformationAbstractMapper<string[]>
    {
        protected override string[] GenerateHeader()
        {
            var header = new string[PositionValueMap.Count];
            header[PositionValueMap[ParticipantNumber]] = ParticipantNumber;
            header[PositionValueMap[EventTime]] = EventTime;
            header[PositionValueMap[EyeTrackingTimeStamp]] = EyeTrackingTimeStamp;
            header[PositionValueMap[PositionInformationFrameNumber]] = PositionInformationFrameNumber;
            header[PositionValueMap[PositionInformationX]] = PositionInformationX;
            header[PositionValueMap[PositionInformationY]] = PositionInformationY;
            header[PositionValueMap[PositionInformationZ]] = PositionInformationZ;

            header[PositionValueMap[RotationInformationX]] = RotationInformationX;
            header[PositionValueMap[RotationInformationY]] = RotationInformationY;
            header[PositionValueMap[RotationInformationZ]] = RotationInformationZ;
            header[PositionValueMap[RotationInformationW]] = RotationInformationW;

            //Center Object
            header[PositionValueMap[CenterHit]] = CenterHit;
            header[PositionValueMap[CenterHitPositionX]] = CenterHitPositionX;
            header[PositionValueMap[CenterHitPositionY]] = CenterHitPositionY;
            header[PositionValueMap[CenterHitPositionZ]] = CenterHitPositionZ;
            header[PositionValueMap[CenterHitGroup]] = CenterHitGroup;

            //Box Object
            header[PositionValueMap[BoxHit]] = BoxHit;
            header[PositionValueMap[BoxHitPositionX]] = BoxHitPositionX;
            header[PositionValueMap[BoxHitPositionY]] = BoxHitPositionY;
            header[PositionValueMap[BoxHitPositionZ]] = BoxHitPositionZ;
            header[PositionValueMap[BoxHitGroup]] = BoxHitGroup;

            //PresentedObject
            header[PositionValueMap[PresentedObjectName]] = PresentedObjectName;
            header[PositionValueMap[PresentedObjectGroup]] = PresentedObjectGroup;

            return header;
        }

        protected override void GenerateBody(List<ParticipantInformationData> genericData,
            ref List<string[]> serializableData)
        {
            foreach (ParticipantInformationData data in genericData)
            {
                var singleDataLine = new string[PositionValueMap.Count];
//                var header = new string[PositionValueMap.Count];
                singleDataLine[PositionValueMap[ParticipantNumber]] = data.ParticipantNumber;
                singleDataLine[PositionValueMap[EventTime]] = data.EventTime;
                singleDataLine[PositionValueMap[EyeTrackingTimeStamp]] = data.EyeTrackingTimeStamp;
                singleDataLine[PositionValueMap[PositionInformationFrameNumber]] =
                    data.PositionRotationInformationData.FrameNumber.ToString(); 
                singleDataLine[PositionValueMap[PositionInformationX]] =
                    data.PositionRotationInformationData.PositionInformation.x.ToString();
                singleDataLine[PositionValueMap[PositionInformationY]] =
                    data.PositionRotationInformationData.PositionInformation.y.ToString();
                singleDataLine[PositionValueMap[PositionInformationZ]] =
                    data.PositionRotationInformationData.PositionInformation.z.ToString();

                singleDataLine[PositionValueMap[RotationInformationX]] =
                    data.PositionRotationInformationData.RotationInformation.x.ToString();
                singleDataLine[PositionValueMap[RotationInformationY]] =
                    data.PositionRotationInformationData.RotationInformation.y.ToString();
                singleDataLine[PositionValueMap[RotationInformationZ]] =
                    data.PositionRotationInformationData.RotationInformation.z.ToString();
                singleDataLine[PositionValueMap[RotationInformationW]] =
                    data.PositionRotationInformationData.RotationInformation.w.ToString();

                //Center Object
                singleDataLine[PositionValueMap[CenterHit]] = data.RayCastData.CenterHit;
                singleDataLine[PositionValueMap[CenterHitPositionX]] = data.RayCastData.CenterHitPosition.x.ToString();
                singleDataLine[PositionValueMap[CenterHitPositionY]] = data.RayCastData.CenterHitPosition.y.ToString();
                singleDataLine[PositionValueMap[CenterHitPositionZ]] = data.RayCastData.CenterHitPosition.z.ToString();
                singleDataLine[PositionValueMap[CenterHitGroup]] = data.RayCastData.CenterHitGroup;

                //Box Object
                singleDataLine[PositionValueMap[BoxHit]] = data.RayCastData.BoxHit;
                singleDataLine[PositionValueMap[BoxHitPositionX]] = data.RayCastData.BoxHitPosition.x.ToString();
                singleDataLine[PositionValueMap[BoxHitPositionY]] = data.RayCastData.BoxHitPosition.y.ToString();
                singleDataLine[PositionValueMap[BoxHitPositionZ]] = data.RayCastData.BoxHitPosition.z.ToString();
                singleDataLine[PositionValueMap[BoxHitGroup]] = data.RayCastData.BoxHitGroup;

                //PresentedObject
                singleDataLine[PositionValueMap[PresentedObjectName]] = data.RayCastData.PresentedObjectName;
                singleDataLine[PositionValueMap[PresentedObjectGroup]] = data.RayCastData.PresentedObjectGroup;
                serializableData.Add(singleDataLine);
            }
        }
    }
}