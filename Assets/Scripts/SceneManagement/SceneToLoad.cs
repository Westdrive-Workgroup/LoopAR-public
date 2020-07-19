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
        switch (sceneToLoad)
        {
            case Scenes.Westbrueck:
                return "Westbrueck";
            case Scenes.Countryroad:
                return "countryroad01";
            case Scenes.Autobahn:
                return "Autobahn";
            case Scenes.MainMenu:
                return "MainMenu";
        }

        return null;
    }
}
