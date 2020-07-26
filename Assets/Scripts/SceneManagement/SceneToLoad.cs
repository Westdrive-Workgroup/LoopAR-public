using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneToLoad : MonoBehaviour
{
    public enum Scenes
    {
        Westbrueck,
        Countryroad,
        Autobahn,
        MainMenu
    }

    private GameObject _currentTarget;
    
    public Scenes sceneToLoad = Scenes.Westbrueck;
    [HideInInspector] public int sceneValue = 0;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _currentTarget)
            return;

        _currentTarget = other.gameObject;

        if (other.GetComponent<ManualController>() != null)
        {
            SceneLoadingHandler.Instance.SceneChange(GetTargetScene(), other);
        }
    }

    private String GetTargetScene()
    {
        switch ((int) sceneToLoad)
        {
            case 0:
                return "Westbrueck";
            case 1:
                return "countryroad01";
            case 2:
                return "Autobahn";
            case 3:
                return "MainMenu";
        }

        return null;
    }
}
