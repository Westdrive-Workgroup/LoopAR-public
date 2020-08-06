using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadExperimentTrigger : MonoBehaviour
{
    private GameObject _currentTarget;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _currentTarget)
            return;

        _currentTarget = other.gameObject;

        if (other.GetComponent<ManualController>() != null)
        {
            SceneLoadingHandler.Instance.LoadExperimentScenes();
        }
    }
}
