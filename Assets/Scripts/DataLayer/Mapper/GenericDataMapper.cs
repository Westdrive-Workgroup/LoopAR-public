using System.Collections.Generic;

namespace LoopAr.DataLayer.Mapper
{
    public abstract class GenericAbstractMapper<Type, Format>
    {
        protected internal List<Format> GenerateSerializableFormat(List<Type> genericData)
        {
            List<Format> serializableData = new List<Format> {GenerateHeader()};
            GenerateBody(genericData, ref serializableData);
            return serializableData;
        }


        protected abstract Format GenerateHeader();
        protected abstract void GenerateBody(List<Type> genericData, ref List<Format> serializableData);
    }
}