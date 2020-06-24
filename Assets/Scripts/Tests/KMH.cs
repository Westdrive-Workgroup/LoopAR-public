using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Microsoft.SqlServer.Server;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Valve.Newtonsoft.Json.Utilities;

public class KMH : MonoBehaviour
{
    public GameObject Car;
    private String canvasText;

    private float roundedSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        roundedSpeed = Mathf.Round(Car.GetComponent<CarController>().GetCurrentSpeedInKmH());
        canvasText = roundedSpeed.ToString();
        GetComponent<TextMeshProUGUI>().text = roundedSpeed + " km/h";
    }
}
