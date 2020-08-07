using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class SceneAssetActivationTrigger : MonoBehaviour
{
    [SerializeField] private bool activateGameObjects;
    private GameObject _currentTarget;
    
    public enum Scenes
    {
        MountainRoad,
        Westbrueck, 
        CountryRoad, 
        Autobahn
    }
    
    public Scenes sceneToActivate = Scenes.Westbrueck;
    
    void Start()
    {
        // this.gameObject.GetComponent<MeshRenderer>().enabled = false;
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
                case Scenes.MountainRoad:
                    MountainRoadManager.Instance.ActivateGameObjects(activateGameObjects);
                    break;
                case Scenes.Westbrueck:
                    WestbrueckManager.Instance.ActivateGameObjects(activateGameObjects);
                    break;
                case Scenes.CountryRoad:
                    CountryRoadManager.Instance.ActivateGameObjects(activateGameObjects);
                    break;
                case Scenes.Autobahn:
                    AutobahnManager.Instance.ActivateGameObjects(activateGameObjects);
                    break;
            }
        }
    }
}
