using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class ChangeCarPath : MonoBehaviour
{
    public void SetParticipantsCarPath(PathCreator newPath, float curveDetector, float precision, float trackerSensitivity, string sceneName)
    {
        this.gameObject.GetComponent<AIController>().SetNewDefaultTrackerSensitivity(trackerSensitivity);
        this.gameObject.GetComponent<AIController>().SetNewPath(newPath, curveDetector, precision, trackerSensitivity, sceneName);
    }
}
