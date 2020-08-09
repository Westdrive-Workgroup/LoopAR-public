using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class ChangeCarPath : MonoBehaviour
{


    public void SetParticipantsCarPath(PathCreator newPath, float curveDetector, float precision, float trackerSensitivity)
    {
        this.gameObject.GetComponent<AIController>().SetNewPath(newPath, curveDetector, precision, trackerSensitivity);
    }
}
