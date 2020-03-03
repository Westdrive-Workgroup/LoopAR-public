using UnityEngine;

namespace LoopAr.Connector
{
    public class EyeTrackingConnector
    {
        /*
         //TODO: Reactivate this code for using Eyeclops
         public static Ray RequestCombinedGazeRay()
        {
            EyeClopsConnector.RequestLastEyePosition(out var combinedEyeGazeVector, out _, out _, out _, out _);
            return combinedEyeGazeVector;
        }

        public static void InitiateEyeTracker(string storePath = null, string prefix = null,
            TimeStampType timeStampType = TimeStampType.RealTime)
        {
            EyeClopsConnector.InitiateEyeTracker(storePath, prefix, timeStampType);
        }

        public static void PauseEyeTracker()
        {
            EyeClopsConnector.PauseEyeTracker();
        }

        public static void ContinueEyeTracker()
        {
            EyeClopsConnector.ContinueEyeTracker();
        }

        public static void StartCalibrateEyeTracker()
        {
            EyeClopsConnector.StartCalibrateEyeTracker();
        }

        public static void StartValidationEyeTracker()
        {
            EyeClopsConnector.StartValidationEyeTracker();
        }

        public static void StoreDataComplete(string storePath = null, string prefix = null)
        {
            EyeClopsConnector.StoreDataComplete(storePath, prefix);
        }

        public static void RequestLastEyePosition(out Ray combinedEyeGazeVector,
            out Vector3 leftEyePosition, out Ray leftEyeGazeVector,
            out Vector3 rightEyePosition, out Ray rightEyeGazeVector)
        {
            EyeClopsConnector.RequestLastEyePosition(out combinedEyeGazeVector,
                out leftEyePosition, out leftEyeGazeVector,
                out rightEyePosition, out rightEyeGazeVector);
        }

        public static float EyeTrackerFrequency()
        {
            return EyeClopsConnector.EyeTrackerFrequency();
        }

        public static void ShowEyeOpenness(out float leftEyeOpenness, out float rightEyeOpenness)
        {
            EyeClopsConnector.ShowEyeOpenness(out leftEyeOpenness, out rightEyeOpenness);
        }

        public static void ResetEyeTrackingData()
        {
            //TODO: Implement the EyeClopsConnector.RestEyeClopsData
            EyeClopsConnector.ResetEyeClopsData();
        }
*/
        public static string GetEyeTrackingTimeStamp()
        {
            return "NotYetImplemented";
//            return EyeClopsConnector.GetEyeClopsTimeStamp();
        }
/*
        public static void SetUsedCamera(Camera usedCamera)
        {
            //TODO: set to EyeClops
        }

        public static void RequestLastFocusedObject(out string objectName, out Vector3 objectPosition)
        {
            EyeClopsConnector.RequestLastFocusedObject(out objectName, out objectPosition);
        }
        */
    }
}