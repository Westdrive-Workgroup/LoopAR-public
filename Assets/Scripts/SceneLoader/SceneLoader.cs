using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
   private Scene _currentScene;
   [SerializeField] public bool checkDuplicateScenes;
   [SerializeField] public bool shouldLoadAdditive;

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
      }

      SceneManager.LoadSceneAsync(sceneIndex, loadAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
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
      }

      SceneManager.LoadSceneAsync(sceneName, loadAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
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
