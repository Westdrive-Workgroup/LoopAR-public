using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualOverrideToGUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Text _text;
    [SerializeField] private AIControler _aiControl;
    
    private void Update()
    {
        _text.text= "Manualoverride: " + _aiControl.manualOverride;
    }
}
