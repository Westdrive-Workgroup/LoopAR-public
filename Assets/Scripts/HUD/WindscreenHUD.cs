using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindscreenHUD : MonoBehaviour
{
    public Text Speed;
    public Text MaxSpeed;
    public Text Date;
    public Text Weather;
    public GameObject WarnDreieck;
    public GameObject NonEventAnzeigen;
    public string weather_forecast = "Baumles, CH \n 15°C"+"\n Teilweise bewölkt";
    public int speedLimit = 70;
    public GameObject Car;
    private Rigidbody RB;
    private int nextUpdate = 1;
    public bool Event;


    // Start is called before the first frame update
    void Start()
    {
        RB = Car.GetComponent<Rigidbody>();
        WarnDreieck.SetActive(false);
        //Weather
        Weather.text = weather_forecast;
    } 
    private void OnTriggerEnter(Collider other){
            Event=true;
            NonEventAnzeigen.SetActive(false);
            WarnDreieck.SetActive(true);  
        }
        

    // Update is called once per frame
    void Update()
    {
        // If the next update is reached
        if (Time.time >= nextUpdate)
        {
            // Change the next update (current second+1)
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            // Call your fonction
            UpdateEverySecond();

        }
       
        // Update is called once per second
        void UpdateEverySecond()
        {
            if(Event == false){
                NonEventAnzeigen.SetActive(true);

            //Speed
                var Velocity = RB.velocity;
                float magnitude = Velocity.magnitude;
                float speed =  Mathf.Round(magnitude* 3.6f);
                Speed.text = speed  + "km/h";
                

                if(speed>=speedLimit){
                        Speed.color = Color.red;
                        MaxSpeed.color = Color.red;
                    }else{
                        Speed.color = Color.cyan;
                        MaxSpeed.color = Color.black;
                    }
                
                //Datum
                var today = System.DateTime.Now;
                Date.text = today.ToString("HH:mm");
                //MaxSpeed
                MaxSpeed.text = speedLimit +"";
                
                
            }else{
                NonEventAnzeigen.SetActive(false);
                WarnDreieck.SetActive(true);                    
                Speed.text = "";
                Date.text = "";
                MaxSpeed.text = "";
                Weather.text = "";
            }
        }
    }
}