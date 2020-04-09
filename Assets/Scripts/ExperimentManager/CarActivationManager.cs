using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class can be called to enable and disable the cars in the scene.
/// </summary>

[DisallowMultipleComponent]
public class CarActivationManager : MonoBehaviour
{
    IsAPedestrian[] _pedestrians;
    private IsACar[] _cars;
    private bool _isActive;

    // Start is called before the first frame update
    void Start()
    {
        _cars = GetComponentsInChildren<IsACar>();
        ChangeCarsActivationState(false);
        _isActive = false;
    }
    
    // Changes the activation states of the cars 
    public void ChangeCarsActivationState()
    {
        if (_isActive)
        {
            ChangeCarsActivationState(false);
        }
        else
        {
            ChangeCarsActivationState(true);
        }

        _isActive = !_isActive;
    }
    
    // Manual override for all cars. Can be set to true or false
    public void ChangeCarsActivationState(bool state)
    {
        foreach (IsACar car in _cars)
        {
            car.gameObject.SetActive(state);
        }
    }
}
