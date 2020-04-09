using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class can be called to enable and disable the pedestrians in the scene.
/// </summary>

[DisallowMultipleComponent]
public class PedestrianActivationManager : MonoBehaviour
{
    IsAPedestrian[] _pedestrians;
    private bool _isActive;

    // Start is called before the first frame update
    void Start()
    {
        _pedestrians = GetComponentsInChildren<IsAPedestrian>();
        ChangePedestriansActivationState(false);
        _isActive = false;
    }
    
    // Changes the activation states of pedestrians 
    public void ChangeState()
    {
        if (_isActive)
        {
            ChangePedestriansActivationState(false);
        }
        else
        {
            ChangePedestriansActivationState(true);
        }

        _isActive = !_isActive;
    }
    
    // Manual override for all pedestrians. Can be set to true or false
    public void ChangePedestriansActivationState(bool state)
    {
        foreach (IsAPedestrian pedestrian in _pedestrians)
        {
            pedestrian.gameObject.SetActive(state);
        }
    }
}
