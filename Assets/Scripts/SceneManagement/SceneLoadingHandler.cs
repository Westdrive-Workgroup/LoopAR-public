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
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignParticipantsCar();
        CameraManager.Instance.SetObjectToFollow(_participantsCar);
    }
    
    public void SceneChange(string targetScene, Collider car)
    {
        car.GetComponent<CarWindows>().SetInsideWindowsAlphaChannel(1);
        // car.GetComponent<CarController>().TurnOffEngine();
        // car.GetComponent<Rigidbody>().useGravity = false;
        // car.GetComponent<AIController>().enabled = false;
        SceneManager.LoadSceneAsync("SceneLoader");
        // ExperimentManager.Instance.SetInitialTransform(Vector3.zero);
        StartCoroutine(LoadScenesAsync(targetScene, car));
    }
    
    private void AssignParticipantsCar()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "SceneLoader":
                _participantsCar = SceneLoadingSceneManager.Instance.GetParticipantsCar();
                break;
            case "safe-mountainroad01":
                _participantsCar = MountainRoadManager.Instance.GetParticipantsCar();
                break;
            case "Westbrueck":
                _participantsCar = WestbrueckManager.Instance.GetParticipantsCar();
                break;
            case "countryroad01":
                _participantsCar = CountryRoadManager.Instance.GetParticipantsCar();
                break;
            case "Autobahn":
                _participantsCar = AutobahnManager.Instance.GetParticipantsCar();
                break;
        }
        _participantsCar.GetComponent<CarWindows>().SetInsideWindowsAlphaChannel(1);
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
        
        // SetUpsInNewScene();
    }

    private void SetUpsInNewScene()
    {
        // _participantsCar.GetComponent<Rigidbody>().useGravity = true;
        // _participantsCar.GetComponent<CarWindows>().SetInsideWindowsAlphaChannel(0);
        // _participantsCar.GetComponent<AIController>().enabled = true;
    }
}
