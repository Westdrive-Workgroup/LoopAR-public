using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCarSound : MonoBehaviour
{
    private AudioSource Speaker;
    public CarController _carController;
    private float SoundSpeed;
    //private float Newspeed;
    private float _currentSpeedinKmH;
    private int Gear;
    private int RPM;
    private float nextUpdate = 2;
    private int currentGear;
    // Start is called before the first frame update
    void Start()
    {
        Speaker = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        _currentSpeedinKmH = Mathf.Round(_carController.GetCurrentSpeedInKmH());
        if (Time.time >= nextUpdate)
        {
            // Change the next update (current second+1)
            nextUpdate = (Mathf.FloorToInt(Time.time) + 2);
            // Call your function
            UpdateEverySecond();
        }
        void UpdateEverySecond()
        {

            currentGear = GearSystem(_currentSpeedinKmH);
        }
        SoundSpeed = 1 + RotationPerMoment(_currentSpeedinKmH) / 50f;
        Speaker.pitch = SoundSpeed;



    }
    public int RotationPerMoment(float _currentSpeedinKmH)
    {
        if (Gear == 1)
        {
            RPM = Mathf.RoundToInt((_currentSpeedinKmH / 10f) * 10);
        }
        else if (Gear == 2)
        {
            RPM = Mathf.RoundToInt((_currentSpeedinKmH / 30f) * 10);
        }
        else if (Gear == 3)
        {
            RPM = Mathf.RoundToInt((_currentSpeedinKmH / 50f) * 10);
        }
        else if (Gear == 4)
        {
            RPM = Mathf.RoundToInt((_currentSpeedinKmH / 70f) * 10);
        }
        else if (Gear == 5)
        {
            RPM = Mathf.RoundToInt((_currentSpeedinKmH / 120f) * 10);
        }
        else if (Gear == 6)
        {
            RPM = Mathf.RoundToInt((_currentSpeedinKmH / 150f) * 10);
        }
        return RPM;
    }
    public int GearSystem(float _currentSpeedinKmH)
    {
        if (_currentSpeedinKmH < 15f)
        {
            if (Gear < 1 || Gear == 2)
            {
                Gear = 1;
            }
            else
            {
                Gear--;
            }
        }
        else if (_currentSpeedinKmH > 15f && _currentSpeedinKmH < 40f)
        {
            if (Gear == 1)
            {
                Gear = 2;
            }
            else if (Gear < 1)
            {
                Gear++;
            }
            else if (Gear > 2)
            {
                Gear--;
            }

        }
        else if (_currentSpeedinKmH > 40f && _currentSpeedinKmH < 60f)
        {
            if (Gear == 2)
            {
                Gear = 3;
            }
            else if (Gear < 2)
            {
                Gear++;
            }
            else if (Gear > 3)
            {
                Gear--;
            }
        }
        else if (_currentSpeedinKmH > 60f && _currentSpeedinKmH < 80f)
        {
            if (Gear == 3)
            {
                Gear = 4;
            }
            else if (Gear < 3)
            {
                Gear++;
            }
            else if (Gear > 4)
            {
                Gear--;
            }
        }
        else if (_currentSpeedinKmH < 115f)
        {
            if (Gear == 4)
            {
                Gear = 5;
            }
            else if (Gear < 4)
            {
                Gear++;
            }
            else if (Gear > 5)
            {
                Gear--;
            }
        }
        else if (_currentSpeedinKmH > 150f)
        {
            Gear = 6;
        }
        return Gear;
    }
}
