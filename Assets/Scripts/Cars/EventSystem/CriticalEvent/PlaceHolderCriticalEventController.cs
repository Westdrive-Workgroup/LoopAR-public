using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHolderCriticalEventController : MonoBehaviour
{
    [SerializeField] private CriticalEventController controller;

    public CriticalEventController GetController()
    {
        return controller;
    }
}
