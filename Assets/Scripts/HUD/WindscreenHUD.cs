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
    public int Speedlimit;
    public GameObject Car;
    private Rigidbody RB;
    private int nextUpdate = 1;
    public bool Event;


    // Start is called before the first frame update
    void Start()
    {
        RB = Car.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        // If the next update is reached
        if (Time.time >= nextUpdate)
        {
            //Debug.Log(Time.time + ">=" + nextUpdate);
            // Change the next update (current second+1)
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            // Call your fonction
            UpdateEverySecond();

        }
        
        // Update is called once per second
        void UpdateEverySecond()
        {
            if(Event == false){
            //Speed
            var Velocity = RB.velocity;
            float speed = Velocity.magnitude;
            Speed.text = Mathf.Round(speed * 3) + "km/h";
            
            //Datum
            var today = System.DateTime.Now;
            Date.text = today.ToString("HH:mm");
            //MaxSpeed
            MaxSpeed.text = Speedlimit + "km/h";
            //Weather
            Weather.text = "Baumles, CH" + "\n" + "15°C" +"\n" + "Teilweise Bewölkt";
            }else{
                Speed.text = "";
                Date.text = "";
                MaxSpeed.text = "";
                Weather.text = "";
            }


        }
    }
}