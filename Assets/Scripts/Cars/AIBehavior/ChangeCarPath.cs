using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class ChangeCarPath : MonoBehaviour
{


    public void SetParticipantsCarPath(PathCreator newPath)
    {
        this.gameObject.GetComponent<AIController>().SetNewPath(newPath);
    }
}
