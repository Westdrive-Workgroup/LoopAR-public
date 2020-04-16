using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class can be called to enable and disable the cars in the scene.
/// </summary>

[DisallowMultipleComponent]
public class ActivationHandler : MonoBehaviour
{
    private bool _isActive;

    // Start is called before the first frame update
    void Start()
    {
        _isActive = false;
    }
    
    // Changes the activation states of the cars 
    public void ChangeActivationState(GameObject targetGroup)
    {
        if (_isActive)
            ChangeActivationState(false, targetGroup);
        else
            ChangeActivationState(true, targetGroup);

        _isActive = !_isActive;
    }
    
    // Manual override for all cars. Can be set to true or false
    public void ChangeActivationState(bool state, GameObject targetGroup)
    {
        foreach (Transform child in targetGroup.transform)
        {
            child.gameObject.SetActive(state);
        }
    }
}
