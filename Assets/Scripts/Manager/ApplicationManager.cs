using System.Collections;
using System.Collections.Generic;
using LOD_avatar.scripts.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationManager : MonoBehaviour
{
    public static ApplicationManager Instance { get; private set; }

    private void Awake()
    {
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
    
    private void  OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "MainMenu":
                TurnOffSpecificExperimentComponents();
                this.GetComponent<CarController>().transform.parent.gameObject.SetActive(false);
                break;
            case "EyetrackingValidation":
                TurnOffSpecificExperimentComponents();
                this.GetComponent<CarController>().transform.parent.gameObject.SetActive(false);
                break;
            case "SeatCalibrationScene":
                TurnOffSpecificExperimentComponents();
                break;
            case "TestDrive2.0":
                TurnOffSpecificExperimentComponents();
                break;
        }
    }

    private void TurnOffSpecificExperimentComponents()
    {
        this.GetComponent<PersistentTrafficEventManager>().gameObject.SetActive(false);
        this.GetComponent<ExperimentManager>().gameObject.SetActive(false);
        this.GetComponent<AvatarLodManager>().gameObject.SetActive(false);
    }
}
