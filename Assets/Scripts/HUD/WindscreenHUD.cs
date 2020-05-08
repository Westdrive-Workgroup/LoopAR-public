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
    public Image SpeedGauge;
    public int speedLimit;
    private float nextUpdate = 1;
    public GameObject NonEventAnzeigen;
    private float speed;
    private bool Event = false;

    public AimedSpeed _aimedSpeed;
    public CarController _carController;
    void Start()
    {
        _aimedSpeed = _carController.gameObject.GetComponent<AimedSpeed>();
    }


    public void DeactivateHUD()
    {
        Event = false;
    }
    public void DriverAlert()
    {
        Event = true;
        //Debug.Log(" bis hier alles gut");
    }

    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Event)
        {
            NonEventAnzeigen.SetActive(true);
        }else{
            NonEventAnzeigen.SetActive(false);
        }
        // If the next update is reached
        if (Time.time >= nextUpdate)
        {
            // Change the next update (current second+1)
            nextUpdate = (Mathf.FloorToInt(Time.time) + 1);
            // Call your function
            UpdateEverySecond();
        }

        // Update is called once per second
        void UpdateEverySecond()
        {
            speed = Mathf.Round(_carController.GetCurrentSpeedInKmH());
            Speed.text = speed + "";
            //speedLimit = Mathf.RoundToInt(_aimedSpeed.GetAimedSpeed());
            speedLimit = Mathf.RoundToInt(_aimedSpeed.GetAimedSpeed() * 3.6f);
            float quotientSpeed = speed / speedLimit;
            SpeedGauge.fillAmount = 0.75f * (Mathf.Round(quotientSpeed * 36) / 36);


            if (speed >= (speedLimit + 5))
            {
                SpeedGauge.color = Color.red;
                Speed.color = Color.red;
                MaxSpeed.color = Color.red;
            }
            else
            {
                SpeedGauge.color = Color.green;
                Speed.color = Color.green;
                MaxSpeed.color = Color.white;
            }

            //Datum
            var today = System.DateTime.Now;
            Date.text = today.ToString("HH:mm");
            //MaxSpeed
            MaxSpeed.text = speedLimit + "";
            //Weather.text = "Westbrueck \n 22°C";
        }
    }
}