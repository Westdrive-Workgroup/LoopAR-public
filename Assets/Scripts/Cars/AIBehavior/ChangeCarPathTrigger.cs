using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCarPathTrigger : MonoBehaviour
{
    private GameObject _currentTarget;

    public enum Scenes
    {
        Westbrueck, 
        CountryRoad, 
        Autobahn
    }
    
    public Scenes sceneToActivate = Scenes.Westbrueck;
    
    void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _currentTarget)
            return;

        _currentTarget = other.gameObject;
        
        if (other.GetComponent<ManualController>() != null)
        {
            switch (sceneToActivate)
            {
                case Scenes.Westbrueck:
                    other.gameObject.GetComponent<ChangeCarPath>().SetParticipantsCarPath(WestbrueckManager.Instance.GetCarPath(), 
                        WestbrueckManager.Instance.GetCurveDetectorStepAhead(), WestbrueckManager.Instance.GetPrecision(), WestbrueckManager.Instance.GetTrackerSensitivity());
                    break;
                case Scenes.CountryRoad:
                    other.gameObject.GetComponent<ChangeCarPath>().SetParticipantsCarPath(CountryRoadManager.Instance.GetCarPath(), 
                        CountryRoadManager.Instance.GetCurveDetectorStepAhead(), CountryRoadManager.Instance.GetPrecision(), CountryRoadManager.Instance.GetTrackerSensitivity());
                    break;
                case Scenes.Autobahn:
                    other.gameObject.GetComponent<ChangeCarPath>().SetParticipantsCarPath(AutobahnManager.Instance.GetCarPath(), 
                        AutobahnManager.Instance.GetCurveDetectorStepAhead(), AutobahnManager.Instance.GetPrecision(), AutobahnManager.Instance.GetTrackerSensitivity());
                    break;
            }
        }
    }
}
