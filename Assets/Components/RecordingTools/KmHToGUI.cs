using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class TextToGUI : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private CarControl _carControl;
    private void Update()
    {
        _text.text= "Km/h: " + Math.Round((_carControl.GetCurrentSpeed()*3.6f),2);
    }
}
