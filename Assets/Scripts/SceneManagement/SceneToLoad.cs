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
            other.GetComponent<CarWindows>().SetInsideWindowsAlphaChannel(1);
            other.GetComponent<CarController>().TurnOffEngine();
            other.GetComponent<AIController>().enabled = false;
            SceneManager.LoadSceneAsync("SceneLoader");
            StartCoroutine(LoadScenesAsync(GetTargetScene()));
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
    
    IEnumerator LoadScenesAsync(string sceneName)
    {
        Debug.Log(sceneName);
        yield return new WaitForSeconds(5);
        Debug.Log("Loading...");
        SceneManager.LoadSceneAsync(sceneName);
    }
}
