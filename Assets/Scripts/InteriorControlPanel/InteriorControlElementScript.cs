using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class InteriorControlElementScript : MonoBehaviour
{
    public Text RPMMeter;
    public Text SpeedMeter;
    public Image SpeedGauge;
    public Image RPMGauge;
    public CarController _carController;
    private AimedSpeed _aimedSpeed;
    public DynamicCarSound DynamicCarSound;
    private float speed;
    private float speedLimit;
    private float RPM;
    // Start is called before the first frame update
    void Start()
    { _aimedSpeed = _carController.gameObject.GetComponent<AimedSpeed>();

    }

    // Update is called once per frame
    void Update()
    {
        speed = Mathf.Round(_carController.GetCurrentSpeedInKmH());
        SpeedMeter.text = speed + "";
        speedLimit = Mathf.RoundToInt(_aimedSpeed.GetRuleSpeed());
        float quotientSpeed = speed / speedLimit;
        SpeedGauge.fillAmount = 0.5f * (Mathf.Round(quotientSpeed * 36) / 36);
        RPM = DynamicCarSound.RotationPerMoment(speed);
        RPMGauge.fillAmount = 0.5f *Mathf.Round(RPM * 10)/300;
        RPMMeter.text = RPM*100 + "";

    }
}
