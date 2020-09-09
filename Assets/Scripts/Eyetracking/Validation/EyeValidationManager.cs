using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EyeValidationManager : MonoBehaviour
{
    public static EyeValidationManager Instance { get; private set; }
    
    public GameObject relativeFixedPoint;
    public GameObject fixationPoint;
    
    private float participantHeight;

    public TextMesh HeadfixationText;
    public TextMesh FixingPointText;
    public TextMesh CounterText;
    public TextMesh SuccessfulValidation;
    public TextMesh FailedValidationText;
    private int _fixationCounterNumber;

    private bool HeadIsFixated;

    private FixationDot _fixationDot;
    
    private bool fixationSuccess;
    [SerializeField] private float validationCountdown;

    private float resettetCountdown;

    private bool ValidationSuccessful;
    
    private bool runningValidation;

    private EyetrackingManager _eyetrackingManager;


    private Vector3 _validationError;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _eyetrackingManager = EyetrackingManager.Instance;
        _eyetrackingManager.NotifyEyeValidationCompletnessObservers += HandleEyeValidationCompletnessStatus;
        
        resettetCountdown = validationCountdown;
        _fixationDot = relativeFixedPoint.GetComponent<FixationDot>();
        
        _fixationDot.NotifyFixationTimeObservers+= HandleFixationCountdownNumber;
        _fixationDot.NotifyLeftTargetObservers+= HandleLeftFixation;
        
        participantHeight = _eyetrackingManager.GetHmdTransform().transform.position.y;
        
        fixationPoint.transform.position= new Vector3(fixationPoint.transform.position.x,participantHeight,fixationPoint.transform.position.z);
        SuccessfulValidation.gameObject.SetActive(false);
        runningValidation = false;
        ValidationSuccessful = false;
        FailedValidationText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!ValidationSuccessful)
        {
            
            if (fixationSuccess)
            {
                if(!runningValidation)
                    validationCountdown -= Time.deltaTime;
                
                if (validationCountdown >= 0)
                {
                    CounterText.text = Mathf.RoundToInt(validationCountdown).ToString();
                    SetPrepareForValidationStatus();
                }
                else
                {
                    SetRunningValidationStatus();
                    EyetrackingManager.Instance.StartValidation(0f);
                }
                
            }
            else
            {
                validationCountdown = resettetCountdown;
                SetFixationStatus();
            }
        }
        
        
    }

    private void ShowFailedValidationStatus(float time)
    {
        StartCoroutine(showingFailedValidation(time));

    }

    private IEnumerator showingFailedValidation(float time)
    {
        validationCountdown = resettetCountdown;
        FailedValidationText.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        FailedValidationText.gameObject.SetActive(false);
        
    }

    private void SetValidationSuccesfulStatus()
    {
        CounterText.gameObject.SetActive(false); 
        FixingPointText.gameObject.SetActive(false);
        HeadfixationText.gameObject.SetActive(false);
        SuccessfulValidation.gameObject.SetActive(true);
        fixationPoint.gameObject.SetActive(false);
    }

    private void SetRunningValidationStatus()
    {
        CounterText.gameObject.SetActive(false);
        HeadfixationText.gameObject.SetActive(false);
        FixingPointText.gameObject.SetActive(false);
        SuccessfulValidation.gameObject.SetActive(false);
        runningValidation = true;
    }
    private void SetPrepareForValidationStatus()
    {
        CounterText.gameObject.SetActive(true);
        HeadfixationText.gameObject.SetActive(false);
        CounterText.color = Color.green;
        FixingPointText.gameObject.SetActive(true);
    }
    private void SetFixationStatus()
    {
        CounterText.gameObject.SetActive(true);
        HeadfixationText.gameObject.SetActive(true);
        CounterText.color = Color.white;
        FixingPointText.gameObject.SetActive(false);
        CounterText.text = _fixationCounterNumber.ToString();
    }
    
    private void HandleFixationCountdownNumber(float number)
    {
        if (!fixationSuccess)
        {
            _fixationCounterNumber = Mathf.RoundToInt(number);

            if (_fixationCounterNumber <= 0)
            {
                Debug.Log("finished Countdown");
                fixationSuccess = true;
            }
        }
    }

    private void HandleLeftFixation(bool LeftTheTarget)
    {
        if (LeftTheTarget)
        {
            fixationSuccess = false;
            if (runningValidation)
            {
                EyetrackingManager.Instance.AbortValidation();
            }
        }
    }

    private void HandleEyeValidationCompletnessStatus(bool wasSuccessful)
    {
        Debug.Log("...and it was also propagated to here... with "+  wasSuccessful);
            
        if (wasSuccessful)
        {
            ValidationSuccessful = true;
            ApplicationManager.Instance.StoreMainMenuLastState("SeatCalibration");
            SetValidationSuccesfulStatus();
            Destroy(relativeFixedPoint);
            CalibrationManager.Instance.StoreValidationErrorData(EyetrackingManager.Instance.GetEyeValidationErrorAngles());
            CalibrationManager.Instance.EyeValidationSuccessful();
            Debug.Log("was successful");
        }
        else
        {
            ShowFailedValidationStatus(5f);
        }
        runningValidation = false;
    }

    public GameObject GetRelativeFixedPoint()
    {
        return relativeFixedPoint;
    }

    private void OnGUI()
    {
        float height = Screen.height;
        float width = Screen.width;
        
        float xB = width / 12f;
        float yB = height / 7f;

        float w = 200f;
        float h = 30f;
        
        GUI.backgroundColor = Color.red;
        if (GUI.Button(new Rect(xB*9, yB, w, h), "Skip Eye Validation"))
        {
            Destroy(relativeFixedPoint);
            ApplicationManager.Instance.StoreMainMenuLastState("SeatCalibration");
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}
