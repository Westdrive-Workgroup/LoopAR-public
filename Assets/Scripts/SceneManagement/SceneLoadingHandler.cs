using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingHandler : MonoBehaviour
{
    public static SceneLoadingHandler Instance { get; private set; }
    
    private void Awake()
    {
        //singleton pattern a la Unity
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SceneChange(string targetScene, Collider participantsCar)
    {
        participantsCar.GetComponent<CarWindows>().SetInsideWindowsAlphaChannel(1);
        participantsCar.GetComponent<CarController>().TurnOffEngine();
        participantsCar.GetComponent<AIController>().enabled = false;
        SceneManager.LoadSceneAsync("SceneLoader");
        StartCoroutine(LoadScenesAsync(targetScene));
    }

    IEnumerator LoadScenesAsync(string targetScene)
    {
        Debug.Log(targetScene);
        yield return new WaitForSeconds(2);
        Debug.Log("Loading...");
        SceneManager.LoadSceneAsync(targetScene);
    }
}
