using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStart : MonoBehaviour
{
    public GameObject[] DeactivatedObjects;

    public GameObject Car;
    // Start is called before the first frame update
    void Start()
    {
        DeactivateObjects();
    }

    private void DeactivateObjects()
    {
        foreach (var inactive in DeactivatedObjects)
        {
            inactive.SetActive(false);
        }
    }

    public void StartTrial()
    {
        Car.SetActive(true);
    }

}
