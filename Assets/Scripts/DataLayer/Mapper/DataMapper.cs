using System.Collections.Generic;
using LoopAr.DataLayer.Mapper.ParticipantInformationDataMapper;
using LoopAr.DataTransferObjects;

namespace LoopAr.DataLayer.Mapper
{
    public static class DataMapper
    {
    private static readonly ParticipantInformationStringMapper ParticipantInformationStringMapper = new ParticipantInformationStringMapper();
        
        public static List<string[]> SerializeParticipantInformationStringData(List<ParticipantInformationData> participantInformationData)
        {
            
            return ParticipantInformationStringMapper.GenerateSerializableFormat(participantInformationData);
        }
    }
}