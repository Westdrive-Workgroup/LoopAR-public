using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeValidationManager : MonoBehaviour
{
    public GameObject fixationPoint;
    
    private float participantHeight;

    public TextMesh HeadfixationText;
    public TextMesh FixingPoint;
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
    // Start is called before the first frame update
    void Start()
    {
        resettetCountdown = validationCountdown;
        _fixationDot = EyetrackingManager.Instance.GetHmdTransform().transform.GetComponentInChildren<FixationDot>();
        
        _fixationDot.NotifyFixationTimeObservers+= HandleFixationCountdownNumber;
        _fixationDot.NotifyLeftTargetObservers+= HandleLeftFixation;
        
        participantHeight = EyetrackingManager.Instance.GetHmdTransform().transform.position.y;
        
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
                validationCountdown -= Time.deltaTime;
                if (validationCountdown >= 0)
                {
                    Debug.Log("prepareValidation");
                    CounterText.text = Mathf.RoundToInt(validationCountdown).ToString();
                    SetPrepareForValidationStatus();
                }
                else
                {
                    Debug.Log("validating");
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
        else
        {
            SetValidationSuccesfulStatus();
            runningValidation = false;
        }
       
        
       
        
        
    }

    private void ShowFailedValidationStatus(float time)
    {
        
        Debug.Log("failed Validation");
        StartCoroutine(showingFailedValidation(time));
        
    }

    private IEnumerator showingFailedValidation(float time)
    {
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        FailedValidationText.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        FailedValidationText.gameObject.SetActive(false);
        
    }

    private void SetValidationSuccesfulStatus()
    {
        CounterText.gameObject.SetActive(false); 
        FixingPoint.gameObject.SetActive(false);
        HeadfixationText.gameObject.SetActive(false);
        SuccessfulValidation.gameObject.SetActive(true);
    }

    private void SetRunningValidationStatus()
    {
        CounterText.gameObject.SetActive(false);
        HeadfixationText.gameObject.SetActive(false);
        FixingPoint.gameObject.SetActive(false);
        SuccessfulValidation.gameObject.SetActive(false);
        runningValidation = true;
    }
    private void SetPrepareForValidationStatus()
    {
        HeadfixationText.gameObject.SetActive(false);
        CounterText.color = Color.green;
        FixingPoint.gameObject.SetActive(true);
    }
    private void SetFixationStatus()
    {
        CounterText.gameObject.SetActive(true);
        HeadfixationText.gameObject.SetActive(true);
        CounterText.color = Color.white;
        FixingPoint.gameObject.SetActive(false);
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
                ShowFailedValidationStatus(5f);
                runningValidation = false;
            }
        }
    }
    
}
