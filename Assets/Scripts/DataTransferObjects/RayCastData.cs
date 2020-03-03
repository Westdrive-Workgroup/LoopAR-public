using UnityEngine;

namespace LoopAr.DataTransferObjects
{
    public class RayCastData
    {
        //Center Object
        internal string CenterHit;
        internal Vector3 CenterHitPosition;
        internal string CenterHitGroup;
        
        //Box Object
        internal string BoxHit;
        internal Vector3 BoxHitPosition;
        internal string BoxHitGroup;
        
        //PresentedObject
        internal string PresentedObjectName;
        internal string PresentedObjectGroup;

    }
}