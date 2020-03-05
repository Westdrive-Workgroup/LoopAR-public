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
    public GameObject Car;
    private Rigidbody RB;
    private int nextUpdate = 1;


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
            //Speed
            var Velocity = RB.velocity;
            float speed = Velocity.magnitude;
            Speed.text = Mathf.Round(speed * 3) + "km/h";
            
            //Datum
            //MaxSpeed
            
            //Weather


        }
    }
}