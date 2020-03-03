using LoopAr.DataTransferObjects;
using UnityEngine;

namespace LoopAr.Internals
{
    public class LoopArRayCaster
    {
        public static void GenerateRayCastObject(Vector3 position, Quaternion rotation, Vector3 forward,
            Camera currentCamera, out RayCastData rayCastData, out HitPositionData hitPositions)
        {
            rayCastData = new RayCastData();
            hitPositions = new HitPositionData {CameraPosition = position, CameraRotation = rotation};

            //TODO Fabian: Check here for all the Layers we use!
            if (Physics.Raycast(currentCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out var centerHit,
                Mathf.Infinity, 1 << 8))
            {
                rayCastData.CenterHit = centerHit.transform.name;
                hitPositions.CenterHitPosition = centerHit.point;
                rayCastData.CenterHitPosition = centerHit.point;
                rayCastData.CenterHitGroup = centerHit.transform.root.name;
            }
            else
            {
                rayCastData.CenterHit = "null";
                hitPositions.CenterHitPosition = Vector3.zero;
                rayCastData.CenterHitPosition = Vector3.zero;
                rayCastData.CenterHitGroup = "null";
            }

            if (Physics.BoxCast(position, new Vector3(5, 5, 5), forward, out var boxHit, rotation,
                Mathf.Infinity, 1 << 8))
            {
                rayCastData.BoxHit = boxHit.transform.name;
                hitPositions.BoxHitPosition = boxHit.point;
                rayCastData.BoxHitPosition = boxHit.point;
                rayCastData.BoxHitGroup = boxHit.transform.root.name;
            }
            else
            {
                rayCastData.BoxHit = "null";
                hitPositions.BoxHitPosition = Vector3.zero;
                rayCastData.BoxHitPosition = Vector3.zero;
                rayCastData.BoxHitGroup = "null";
            }

            Collider[] environment = Physics.OverlapBox(position, new Vector3(5, 5, Mathf.Infinity), rotation, 1 << 8);
            rayCastData.PresentedObjectName = "";
            rayCastData.PresentedObjectGroup = "";
            if (environment.Length == 0)
            {
                rayCastData.PresentedObjectName += "null";
                rayCastData.PresentedObjectGroup += "null";
            }
            else
            {
                foreach (Collider presentedObject in environment)
                {
                    rayCastData.PresentedObjectName += (presentedObject.transform.name + "*");
                    rayCastData.PresentedObjectGroup += (presentedObject.transform.root.name + "*");
                }
            }
        }
    }
}