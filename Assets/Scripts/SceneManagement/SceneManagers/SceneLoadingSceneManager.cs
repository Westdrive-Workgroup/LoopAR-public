using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadingSceneManager : MonoBehaviour
{
    public static SceneLoadingSceneManager Instance { get; private set; }

    [SerializeField] private GameObject participantsCar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public GameObject GetParticipantsCar()
    {
        return participantsCar;
    }
}
