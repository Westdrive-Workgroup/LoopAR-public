using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingHandler : MonoBehaviour
{
    public static SceneLoadingHandler Instance { get; private set; }

    private GameObject _participantsCar;
    
    public delegate void OnInitiateLoadingScene();
    public event OnInitiateLoadingScene NotifySceneLoadingStartObservers;

    
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

        if (_participantsCar !=null)
        {
            if (SceneManager.GetActiveScene().name != "SceneLoader")
                _participantsCar.GetComponent<CarWindows>().SetInsideWindowsAlphaChannel(0);
            else
                _participantsCar.GetComponent<CarWindows>().SetInsideWindowsAlphaChannel(1);
        }
    }
    
    public void SceneChange(string targetScene, Collider car)
    {
        car.GetComponent<CarWindows>().SetInsideWindowsAlphaChannel(1);
        CameraManager.Instance.FadeOut();
        StartCoroutine(LoadSceneLoaderScenesAsync(targetScene));
    }
    public void SceneChange(string targetScene)
    {
        CameraManager.Instance.FadeOut();
        StartCoroutine(LoadScenesAsync(targetScene));
    }
    
    IEnumerator LoadSceneLoaderScenesAsync(string targetScene)
    {
        Debug.Log("SceneLoader");
        // CameraManager.Instance.FadeIn();
        yield return new WaitForSeconds(1);
        Debug.Log("Loading...");
        
        AsyncOperation operation = SceneManager.LoadSceneAsync("SceneLoader");

        // NotifySceneLoadingStartObservers?.Invoke();
        // CameraManager.Instance.OnSceneLoadingInitiated();
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            Debug.Log(operation.progress);

            yield return null;
        }

        CameraManager.Instance.OnSceneLoaded();
        StartCoroutine(LoadScenesAsync(targetScene));
    }
    
    IEnumerator LoadScenesAsync(string targetScene)
    {
        Debug.Log(targetScene);
        yield return new WaitForSeconds(2);
        Debug.Log("Loading...");
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(targetScene);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            Debug.Log(operation.progress);

            if (progress >= .9f)
            {
                CameraManager.Instance.FadeOut();
            }
            
            yield return null;
        }
        
        CameraManager.Instance.OnSceneLoaded();
    }
    
    
    private void AssignParticipantsCar()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "SceneLoader":
                _participantsCar = SceneLoadingSceneManager.Instance.GetParticipantsCar();
                break;
            case "SeatCalibrationScene":
                _participantsCar = SeatCalibrationManager.Instance.GetParticipantsCar();
                break;
            case "TestDrive2.0":
                _participantsCar = TrainingHandler.Instance.testEventManager.GetParticipantCar();
                break;
            case "MountainRoad":
                _participantsCar = MountainRoadManager.Instance.GetParticipantsCar();
                break;
            case "Westbrueck":
                _participantsCar = WestbrueckManager.Instance.GetParticipantsCar();
                break;
            case "CountryRoad":
                _participantsCar = CountryRoadManager.Instance.GetParticipantsCar();
                break;
            case "Autobahn":
                _participantsCar = AutobahnManager.Instance.GetParticipantsCar();
                break;
        }
    }

    public GameObject GetParticipantsCar()
    {
        AssignParticipantsCar();
        return _participantsCar;
    }
}


