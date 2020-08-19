using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataSavingTrigger : MonoBehaviour
{
    private string _oldScene;
    
    void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        _oldScene = SceneManager.GetActiveScene().name;
        StartCoroutine(IsSceneChanged());
    }

    IEnumerator IsSceneChanged()
    {
        while (SceneManager.GetActiveScene().name == _oldScene)
        {
            yield return null;
        }
        
        SavingManager.Instance.SaveDataAndStartRecordingAgain(_oldScene);
    }
}
