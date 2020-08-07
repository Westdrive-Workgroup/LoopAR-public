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
                    this.gameObject.GetComponent<ChangeCarPath>().SetParticipantsCarPath(WestbrueckManager.Instance.GetCarPath());
                    break;
                case Scenes.CountryRoad:
                    this.gameObject.GetComponent<ChangeCarPath>().SetParticipantsCarPath(CountryRoadManager.Instance.GetCarPath());
                    break;
                case Scenes.Autobahn:
                    this.gameObject.GetComponent<ChangeCarPath>().SetParticipantsCarPath(AutobahnManager.Instance.GetCarPath());
                    break;
            }
        }
    }
}
