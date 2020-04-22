using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
   
   #region Singelton

   public static SceneLoader Instance { get; private set; }

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

   #endregion
   
   private Scene _currentScene;
   [SerializeField] public bool checkDuplicateScenes;
   [SerializeField] public bool shouldLoadAdditive;

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.Alpha0))
      {
         AsyncLoad(0, shouldLoadAdditive);
      }
      
      if (Input.GetKeyDown(KeyCode.Alpha1))
      {
         AsyncLoad(1, shouldLoadAdditive);
      }
      
      if (Input.GetKeyDown(KeyCode.Alpha2))
      {
        AsyncLoad(2, shouldLoadAdditive);
      }
      
      if (Input.GetKeyDown(KeyCode.Alpha3))
      {
         AsyncLoad(3, shouldLoadAdditive);
      }
      
      if (Input.GetKeyDown(KeyCode.Alpha4))
      {
         AsyncLoad(4, shouldLoadAdditive);
      }
      
      if (Input.GetKeyDown(KeyCode.Alpha5))
      {
         AsyncLoad(5, shouldLoadAdditive);
      }
      
      if (Input.GetKeyDown(KeyCode.Alpha6))
      {
         AsyncLoad(6, shouldLoadAdditive);
      }
      
      if (Input.GetKeyDown(KeyCode.Alpha7))
      {
         AsyncLoad(7, shouldLoadAdditive);
      }
      
      if (Input.GetKeyDown(KeyCode.Alpha8))
      {
         AsyncLoad(8, shouldLoadAdditive);
      }
      
      if (Input.GetKeyDown(KeyCode.Alpha9))
      {
         AsyncLoad(9, shouldLoadAdditive);
      }
   }

   private void OnEnable()
   {
      SceneManager.sceneLoaded += OnSceneLoaded;
      SceneManager.sceneUnloaded += OnSceneUnloaded;
   }

   // Asynchronously loads the scene at buildIndex 'sceneIndex'. If checkDuplicateScenes is 'true' it will check if the scene is already loaded.
   public void AsyncLoad(int sceneIndex)
   {
     AsyncLoad(sceneIndex, false);
   }
   
   // Asynchronously loads the scene with name 'sceneName'. If checkDuplicateScenes is 'true' it will check if the scene is already loaded.
   public void AsyncLoad(string sceneName)
   {
     AsyncLoad(sceneName, false);
   }

   public void AsyncLoadAdditive(int sceneIndex)
   {
      AsyncLoad(sceneIndex, true);
   }
   
   public void AsyncLoadAdditive(string sceneName)
   {
      AsyncLoad(sceneName, true);
   }
   
   // Asynchronously loads the scene at buildIndex 'sceneIndex'. If loadAdditive is true, the scene will be loaded additive, else single.
   // If checkDuplicateScenes is 'true' it will check if the scene is already loaded.
   public void AsyncLoad(int sceneIndex, bool loadAdditive)
   {
      if (checkDuplicateScenes)
      {
         if (CheckIfSceneIsLoaded(SceneManager.GetSceneByBuildIndex(sceneIndex)) != true)
         {
            SceneManager.LoadSceneAsync(sceneIndex, loadAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
         }
         else
         {
            Debug.Log("Scene already loaded!!!");
         }
      }
      else
      {
         SceneManager.LoadSceneAsync(sceneIndex, loadAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
      }
   }
   
   // Asynchronously loads the scene with name 'sceneName'. If loadAdditive is true, the scene will be loaded additive, else single.
   // If checkDuplicateScenes is 'true' it will check if the scene is already loaded.
   public void AsyncLoad(string sceneName, bool loadAdditive)
   {
      if (checkDuplicateScenes)
      {
         if (CheckIfSceneIsLoaded(SceneManager.GetSceneByName(sceneName)) != true)
         {
            SceneManager.LoadSceneAsync(sceneName, loadAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
         }
         else
         {
            Debug.Log("Scene already loaded!!!");
         }
      }
      else
      {
         SceneManager.LoadSceneAsync(sceneName, loadAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
      }
   }

   // Methods to check if a scene was already loaded. Needs a scene and returns true if the scene if already loaded
   // and false otherwise.
   private bool CheckIfSceneIsLoaded(Scene scene)
   {
      return scene.isLoaded;
   }

   private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
   {
      // if (mode != LoadSceneMode.Additive)
      // {
      //    _currentScene = SceneManager.GetActiveScene();
      //    Debug.Log("Current Active Scene is: " + _currentScene.name);
      //    Debug.Log("Scene at index 1 is: " + SceneManager.GetSceneByBuildIndex(1).name);
      //    return;
      // }
      //
      // SceneManager.SetActiveScene(scene);
      // Debug.Log("Active Scene is:" + scene.name);
      // Debug.Log("Old Scene is:" + _currentScene.name);
      //
      // if (_currentScene.buildIndex != 0)
      // {
      //    SceneManager.UnloadSceneAsync(_currentScene);
      // }
      //
      // _currentScene = scene;
   }

   private void OnSceneUnloaded(Scene arg0)
   { 
      throw new NotImplementedException();
   }

   private void OnDisable()
   {
      SceneManager.sceneLoaded -= OnSceneLoaded;
      SceneManager.sceneUnloaded -= OnSceneUnloaded;
   }
}
