using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MountainRoadManager : MonoBehaviour
{
    public static MountainRoadManager Instance { get; private set; }

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
