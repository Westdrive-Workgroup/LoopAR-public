using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingHandler : MonoBehaviour
{
    public static SceneLoadingHandler Instance { get; private set; }

    private GameObject _participantsCar;
    
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
    
    public void SceneChange(string targetScene, Collider car)
    {
        // _participantsCar = car.gameObject;
        car.GetComponent<CarWindows>().SetInsideWindowsAlphaChannel(1);
        // car.GetComponent<CarController>().TurnOffEngine();
        car.GetComponent<Rigidbody>().useGravity = false;
        car.GetComponent<AIController>().enabled = false;
        SceneManager.LoadSceneAsync("SceneLoader");
        StartCoroutine(LoadScenesAsync(targetScene, car));
    }

    IEnumerator LoadScenesAsync(string targetScene, Collider car)
    {
        Debug.Log(targetScene);
        yield return new WaitForSeconds(2);
        Debug.Log("Loading...");
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(targetScene);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            Debug.Log(operation.progress);

            yield return null;
        }
        
        car.GetComponent<CarWindows>().SetInsideWindowsAlphaChannel(0);
        car.GetComponent<AIController>().enabled = true;


        // SetUpsInNewScene();
    }

    private void SetUpsInNewScene()
    {
        _participantsCar.GetComponent<CarWindows>().SetInsideWindowsAlphaChannel(0);
        _participantsCar.GetComponent<CarController>().TurnOnEngine();
        _participantsCar.GetComponent<AIController>().enabled = true;
    }
}
