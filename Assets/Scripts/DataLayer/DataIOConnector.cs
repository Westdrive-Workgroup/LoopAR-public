using System.Collections.Generic;
using LoopAr.DataLayer.DeSerialization;
using LoopAr.DataLayer.Mapper;
using LoopAr.DataTransferObjects;
using LoopAr.Internals;

namespace LoopAr.DataLayer
{
    public class DataIOConnector
    {
        public void SaveParticipantData(List<ParticipantInformationData> participantInformationData, string fileAddress,
            string fileNamePrefix)
        {
            //TODO: find a better FileIdentifierName Place: ParticipantInformationData
            CsvDeSerializer.WriteCSVFile(DataMapper.SerializeParticipantInformationStringData(participantInformationData),
                fileAddress, fileNamePrefix + FileAdditions.FilePrefixAndSuffixSeparator + "ParticipantInformationData", FileEndings.Csv);
        }
    }
}