using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExperimentLoader : MonoBehaviour
{
   private Scene _currentScene;
   [SerializeField] public bool checkDuplicateScenes;

   private void OnEnable()
   {
      SceneManager.sceneLoaded += OnSceneLoaded;
      SceneManager.sceneUnloaded += OnSceneUnloaded;
   }

   // Asynchronously loads the scene at buildIndex 'sceneIndex'. If checkDuplicateScenes is 'true' it will check if the scene is already loaded.
   public void AsyncLoad(int sceneIndex)
   {
      if (checkDuplicateScenes == true)
      {
         if (CheckIfSceneIsLoaded(SceneManager.GetSceneByBuildIndex(sceneIndex)) != true)
         {
            SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
         }
      }
      
      SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
   }
   
   // Asynchronously loads the scene with name 'sceneName'. If checkDuplicateScenes is 'true' it will check if the scene is already loaded.
   public void AsyncLoad(string sceneName)
   {
      if (checkDuplicateScenes == true)
      {
         if (CheckIfSceneIsLoaded(SceneManager.GetSceneByName(sceneName)) != true)
         {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
         }
      }
      
      SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
   }

   // Methods to check if a scene was already loaded. Needs a scene and returns true if the scene if already loaded
   // and false otherwise.
   private bool CheckIfSceneIsLoaded(Scene scene)
   {
         if (scene.isLoaded)
         {
            return true;
         }

         return false;
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
