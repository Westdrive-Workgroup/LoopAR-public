using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Advance : MonoBehaviour
{
    [Header("Event")]
    [SerializeField]
    [Header("Pre Experiment")]


    public AudioSource audioSource;

    public RawImage WarningTriangle;
    public Text WarningText;
    public AudioClip WarningVerbalSound, WarningSound;
    public RawImage YouDriving;
    public Text YouDrivingText;


    [Header("No Event")]
    public Text Speed;
    public Text MaxSpeed, Date, Weather;
    public Image SpeedGauge;
    public RawImage Circle;
    private bool AIDrivingBool = false;
    public RawImage AIDriving;
    public Text AIDrivingText;

    [Header("Event End")]
    public RawImage TorBackSign;
    public Text TorBackText;
    public AudioClip TorBackSound;
    [Header("Classes")]
    public AimedSpeed _aimedSpeed;
    public CarController _carController;

    [Space]

    [SerializeField]
    [Header("Event")]
    [Header("Experiment")]

    private bool IsEvent;
    public bool TimeShow, SpeedShow, SpeedLimitShow, ShowRealTime, UseSound, UseSoundTOR, ShowOutline, ShowTriangle;
    public string ShowFakeTime;
    public float TimeTillWarningSign;
    public float BlinkingFrequence = 3;
    public float BlinkingForTime = 2;
    public bool BlinkingText;
    public bool BlinkingTriangle;
    public float TimeTillWarningVoice = 0.7f;
    public float TimeTillWarningSound = 0, WarningSignDuration = 2;
    [SerializeField] private List<GameObject> _eventObjectsToMark;
    [SerializeField] private List<GameObject> _highlightedObjects;

    [Header("No Event AI Drive")]
    public bool ShowLocation;
    [SerializeField] private int speedLimit;

    [SerializeField] private float nextUpdate = 1;

    [SerializeField] private float speed;
    public float TorBackBlinkingFrequency = 3;
    public float TorBackBlinkingLength = 2, TorBackDuration = 2;
    public bool TorBackBlinkingText, TorBackBlinkingImage;
    [Header("No Event Manual Drive")]
    public bool ManualDriving;
    private bool EventDriving;
    private float BlinkFreq;
    private float BlinkLength;
    private bool playTOR = false;

    // Start is called before the first frame update
    void Start()
    {
        _aimedSpeed = _carController.gameObject.GetComponent<AimedSpeed>();
        WarningText.enabled = false;
        WarningTriangle.enabled = false;
        TorBackSign.enabled = false;
        TorBackText.enabled = false;
        if (!IsEvent)
        {
            if (!ManualDriving)
            {
                AIDrive(false);
            }
            else
            {
                ManualDrive();
                _carController.gameObject.GetComponent<ControlSwitch>().SwitchControl(ManualDriving);
            }
            //Debug.Log("Dreieck Aus");
        }

    }

    public void ActivateHUD(GameObject testAccidentObject)
    {
        List<GameObject> ObjectToMark = new List<GameObject>();
        ObjectToMark.Add(testAccidentObject);
        ActivateHUD(ObjectToMark);

    }

    public void ActivateHUD(List<GameObject> testAccidentSubjects)
    {
        _eventObjectsToMark = testAccidentSubjects;
        if (ShowOutline) MarkObjects();
    }

    private void MarkObjects()
    {
        foreach (var eventObject in _eventObjectsToMark)
        {
            var outline = eventObject.AddComponent<Outline>();

            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.OutlineColor = Color.red;
            outline.OutlineWidth = 5f;
            _highlightedObjects.Add(eventObject);
        }
    }
    public void DeactivateHUD(bool playTOR)
    {
        IsEvent = false;
        _eventObjectsToMark.Clear();
        foreach (GameObject objet in _highlightedObjects)
        {
            Destroy(objet.GetComponent<Outline>());
        }
        _highlightedObjects.Clear();
        if (!ManualDriving)
        {
            AIDrive(playTOR);
        }
        else
        {
            ManualDrive();
        }
    }

    public void DriverAlert()
    {
        IsEvent = true;
        AIDrivingBool = false;
        if (!EventDriving)
        {
            EventDrive();
        }

    }
    public void BlankState()
    {
        Weather.enabled = false;
        Date.enabled = false;
        Speed.enabled = false;
        SpeedGauge.enabled = false;
        MaxSpeed.enabled = false;
        Circle.enabled = false;
        AIDrivingBool=false;
        AIDrivingText.enabled = false;
        AIDriving.enabled = false;
        YouDriving.enabled = false;
        YouDrivingText.enabled = false;
    }
    public void MakeNoiseTOR()
    {
        StartCoroutine(SoundManagerTOR());
    }
    public void MakeNoise()
    {
        StartCoroutine(SoundManagerWarning());
    }
    public void DrawMeLikeOnOfYourFrenchGirls()
    {
        if (!TimeShow) Date.enabled = false;
        if (!SpeedShow) Speed.enabled = false;
        if (!SpeedShow) SpeedGauge.enabled = false;
        if (!SpeedLimitShow) MaxSpeed.enabled = false;
        if (!SpeedLimitShow) Circle.enabled = false;
        Weather.enabled = false;
        DrawOnlyTriangle();

        AIDrivingText.enabled = false;
        AIDriving.enabled = false;

    }
    public void DrawOnlyTriangle()
    {
        if (BlinkingText || BlinkingTriangle)
        {
            BlinkFreq = BlinkingFrequence;
            BlinkLength = BlinkingForTime;
            StartCoroutine(Blink(BlinkFreq, BlinkLength));
        }
        else
        {
            if (ShowTriangle) StartCoroutine(ShowForSeconds(WarningSignDuration));
        }
    }
    public void BlendInNED()
    {
        if (ShowLocation) { Weather.enabled = true; }
        else
        {
            Weather.enabled = false;
        }
        Speed.enabled = true;
        SpeedGauge.enabled = true;
        MaxSpeed.enabled = true;
        Circle.enabled = true;
        Date.enabled = true;
    }
    public void BlendOutNED()
    {
        Weather.enabled = false;
        
        Speed.enabled = false;
        SpeedGauge.enabled = false;
        MaxSpeed.enabled = false;
        Circle.enabled = false;
        Date.enabled = false;

    }
    public void DrawMeLikeOnOfYourFrenchGirlsTOR()
    {
        YouDriving.enabled = false;
        YouDrivingText.enabled = false;
        if (TorBackBlinkingImage || TorBackBlinkingText)
        {
            if (nextUpdate > 10)
            {
                BlinkFreq = TorBackBlinkingFrequency;
                BlinkLength = TorBackBlinkingLength;
                StartCoroutine(Blink(BlinkFreq, BlinkLength));
            }
        }
        else
        {
            StartCoroutine(ShowForSeconds(TorBackDuration));
        }
        BlendInNED();

    }
    public void AIDrive(bool playTOR)
    {
        AIDrivingBool = true;
        ManualDriving = false;

        EventDriving = false;

        //Take over request back Image && Text && Sound -> maybe Blinking 
        //start NonEventDisplays
        //start AI DrivingSign
        // Debug.Log("Aidrive start");

        if (playTOR)
        {
            MakeNoiseTOR();
            DrawMeLikeOnOfYourFrenchGirlsTOR();
        }
        StartCoroutine(ShowAfterSeconds());
        Speed.enabled = true;
        SpeedGauge.enabled = true;
        MaxSpeed.enabled = true;
        Circle.enabled = true;
        Date.enabled = true;

    }
    public void ManualDrive()
    {        //You are driving
        ManualDriving = true;
        AIDrivingBool = false;

        StartCoroutine(ShowForSeconds(TorBackDuration));
        //start NonEventDisplays

        Date.enabled = true;
        Speed.enabled = true;
        SpeedGauge.enabled = true;
        MaxSpeed.enabled = true;
        Circle.enabled = true;
        if (ShowLocation) { Weather.enabled = true; }
        else
        {
            Weather.enabled = false;
        }
        AIDrivingText.enabled = false;
        AIDriving.enabled = false;
        //No TORBack no Sound 
        StartCoroutine(ShowAfterSeconds());
    }
    public void EventDrive()
    {
        EventDriving = true;

        //Warning Sound && Triangle && Text && Blinking
        //Verbal Warning
        //
        DrawMeLikeOnOfYourFrenchGirls();
        //Debug.Log("Warum bin ich hier?");
        MakeNoise();
        StartCoroutine(ShowAfterSeconds());

    }
    private IEnumerator SoundManagerTOR()
    {
        IsEvent = false;
        audioSource.Stop();
        yield return new WaitForSeconds(0.2f);
        //audioSource.clip = ;
        audioSource.PlayOneShot(TorBackSound);
        yield return new WaitForSeconds(120f);

        yield return null;
        StopCoroutine(SoundManagerTOR());
    }
    private IEnumerator SoundManagerWarning()
    {
        
            yield return new WaitForSeconds(TimeTillWarningSound);
            audioSource.PlayOneShot(WarningSound);
            yield return new WaitForSeconds(TimeTillWarningVoice);
            audioSource.PlayOneShot(WarningVerbalSound);
            yield return new WaitForSeconds(1.2f);
            yield return new WaitForSeconds(0.7f);

            audioSource.Stop();
            StopCoroutine(SoundManagerWarning());
        
    }
    private IEnumerator ShowAfterSeconds()
    {
        yield return new WaitForSeconds(3f);
        if (IsEvent || ManualDriving)
        {

            YouDriving.enabled = true;
            YouDrivingText.enabled = true;
        }

        if (AIDrivingBool)
        {
            AIDrivingText.enabled = true;
            AIDriving.enabled = true;
        }
    }
    private IEnumerator ShowForSeconds(float TorBackDuration)
    {
        if (IsEvent)
        {
            yield return new WaitForSeconds(TimeTillWarningSign);
            WarningText.enabled = true;
            WarningTriangle.enabled = true;
            TorBackSign.enabled = false;
            TorBackText.enabled = false;
        }
        else
        {
            // Debug.Log("TOR");
            WarningText.enabled = false;
            WarningTriangle.enabled = false;
            if (nextUpdate > 10)
            {
                TorBackSign.enabled = true;
                TorBackText.enabled = true;
                // Debug.Log(" Tor startet");
            }

        }
        // Debug.Log("Tor sollte Enden");
        yield return new WaitForSeconds(TorBackDuration);
        WarningText.enabled = false;
        WarningTriangle.enabled = false;
        TorBackSign.enabled = false;
        TorBackText.enabled = false;


    }
    private IEnumerator Blink(float BlinkFreq, float BlinkLength)
    {
        WarningText.enabled = false;
        WarningTriangle.enabled = false;
        TorBackSign.enabled = false;
        TorBackText.enabled = false;
        int i = 0;
        float EndI = BlinkFreq * BlinkLength;
        if (!BlinkingText || !BlinkingTriangle) StartCoroutine(ShowForSeconds(BlinkLength));
        if (!TorBackBlinkingImage || !TorBackBlinkingText) StartCoroutine(ShowForSeconds(BlinkLength));

        while (i < EndI)
        {

            if (IsEvent)
            {

                if (BlinkingText)
                {
                    WarningText.enabled = true;
                }
                if (BlinkingTriangle)
                {
                    WarningTriangle.enabled = true;
                }

            }
            else
            {
                //yield return new WaitForSeconds(3f);
                if (TorBackBlinkingImage) TorBackSign.enabled = true;
                if (TorBackBlinkingText) TorBackText.enabled = true;
            }
            i++;
            yield return new WaitForSeconds(0.5f / BlinkFreq);

            if (IsEvent)
            {
                if (BlinkingText) WarningText.enabled = false;
                if (BlinkingTriangle) WarningTriangle.enabled = false;
            }
            else
            {
                if (TorBackBlinkingImage) TorBackSign.enabled = false;
                if (TorBackBlinkingText) TorBackText.enabled = false;
            }
            yield return new WaitForSeconds(0.5f / BlinkFreq);
            //yield return null;
        }
        WarningText.enabled = false;
        WarningTriangle.enabled = false;
        TorBackSign.enabled = false;
        TorBackText.enabled = false;
    }

    // Update is called once per frame

    void FixedUpdate()
    {
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
            //speedLimit = Mathf.RoundToInt(_aimedSpeed.GetRuleSpeed());
            speedLimit = Mathf.RoundToInt(_aimedSpeed.GetRuleSpeed());
            float quotientSpeed = speed / speedLimit;
            SpeedGauge.fillAmount = 0.75f * (Mathf.Round(quotientSpeed * 36) / 36);


            if (speed >= (speedLimit + 5))
            {
                SpeedGauge.color = Color.red;
                Speed.color = Color.red;
                //MaxSpeed.color = Color.red;
            }
            else
            {
                SpeedGauge.color = Color.green;
                Speed.color = Color.green;
                MaxSpeed.color = Color.white;
            }

            //Datum
            var today = System.DateTime.Now;
            if (ShowRealTime)
            {
                Date.text = today.ToString("HH:mm");
            }
            else { Date.text = ShowFakeTime; }
            //MaxSpeed
            MaxSpeed.text = speedLimit + "";
            Weather.text = "Westbrueck \n 22°C";
        }
    }
}
