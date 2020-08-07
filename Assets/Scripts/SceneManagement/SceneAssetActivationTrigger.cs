using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class SceneAssetActivationTrigger : MonoBehaviour
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
                    // todo
                    break;
                case Scenes.CountryRoad:
                    // todo
                    break;
                case Scenes.Autobahn:
                    // todo
                    break;
            }
        }
    }
    
    
}
