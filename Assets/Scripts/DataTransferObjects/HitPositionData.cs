using UnityEngine;

namespace LoopAr.DataTransferObjects
{
    
    /// <summary>
    /// This class is needed for the Visualization, where the Hit was in the environment
    ///  But maybe we don't need this information
    /// </summary>
    public class HitPositionData
    {
        public Vector3 CenterHitPosition;
        public Vector3 BoxHitPosition;
        public Vector3 CameraPosition;
        public Quaternion CameraRotation;
    }
}