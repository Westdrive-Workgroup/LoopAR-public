using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionManager : MonoBehaviour
{
    public static ConditionManager Instance { get; private set; }

    private List<GameObject> _eventObjects;
    private bool _playTakeOverRequest;
    
    private enum Conditions
    {
        Default,
        FullLoopAR,
        BaseCondition,
        HUDOnly,
        AudioOnly
    }

    private Conditions _condition = Conditions.Default;
    
    #region PrivateMethods
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        /*else
        {
            Destroy(gameObject);
        }*/
    }

    private void DriverAlertCondition()
    {
        switch (_condition)
        {
            case Conditions.FullLoopAR:
                SceneLoadingHandler.Instance.GetParticipantsCar().GetComponentInChildren<HUD_Advance>().DriverAlert();
                break;
            case Conditions.HUDOnly:
                // SceneLoadingHandler.Instance.GetParticipantsCar().GetComponentInChildren<HUD_Advance>().DrawOnlyTriangle();    // todo implement test
                SceneLoadingHandler.Instance.GetParticipantsCar().GetComponentInChildren<HUD_Advance>().DrawTriangle();
                break;
            case Conditions.AudioOnly:
                // SceneLoadingHandler.Instance.GetParticipantsCar().GetComponentInChildren<HUD_Advance>().OnlyAudioHUD();    // todo implement test
                SceneLoadingHandler.Instance.GetParticipantsCar().GetComponentInChildren<HUD_Advance>().PlayWarningAndSiren();
                break;
            case Conditions.BaseCondition:
                break;
            case Conditions.Default:
                Debug.Log("<color=red>This condition is not generated randomly!!!</color>");
                break;
        }
    }

    private void StartEventCondition()
    {
        switch (_condition)
        {
            case Conditions.FullLoopAR:
            case Conditions.HUDOnly:
                SceneLoadingHandler.Instance.GetParticipantsCar().GetComponent<ControlSwitch>().GetComponentInChildren<HUD_Advance>().ActivateHUD(_eventObjects);    // todo test
                break;
            case Conditions.AudioOnly:
            case Conditions.BaseCondition:
                break;
            case Conditions.Default:
                Debug.Log("<color=red>This condition is not generated randomly!!!</color>");
                break;
        }
    }

    private void EndEventCondition()
    {
        switch (_condition)
        {
            case Conditions.FullLoopAR:
                SceneLoadingHandler.Instance.GetParticipantsCar().GetComponentInChildren<HUD_Advance>().DeactivateHUD(_playTakeOverRequest);
                break;
            case Conditions.HUDOnly:
                // SceneLoadingHandler.Instance.GetParticipantsCar().GetComponentInChildren<HUD_Advance>().DeactivateHUD(false);
                if (_playTakeOverRequest)
                {
                    SceneLoadingHandler.Instance.GetParticipantsCar().GetComponentInChildren<HUD_Advance>().DeactivateHUDSoundless(true);
                }
                else
                {
                    SceneLoadingHandler.Instance.GetParticipantsCar().GetComponentInChildren<HUD_Advance>().DeactivateHUDSoundless(false);
                }
                break;
            case Conditions.AudioOnly:
                if (_playTakeOverRequest)
                {
                    // SceneLoadingHandler.Instance.GetParticipantsCar().GetComponentInChildren<HUD_Advance>().OnlyAudioHUDTOR(); // todo implement test
                    SceneLoadingHandler.Instance.GetParticipantsCar().GetComponentInChildren<HUD_Advance>().PlayTakingBackControl();
                }                
                break;
            case Conditions.BaseCondition:
                break;
            case Conditions.Default:
                Debug.Log("<color=red>This condition is not generated randomly!!!</color>");
                break;
        }
    }
    
    #endregion

    #region PublicMethods

    public void DriverAlert()
    {
        DriverAlertCondition();
    }

    public void StartEvent(List<GameObject> eventObjects)
    {
        _eventObjects = eventObjects;
        StartEventCondition();
    }

    public void EndEvent(bool successful)
    {
        _playTakeOverRequest = successful;
        EndEventCondition();
    }
    
    public void SetExperimentalCondition(string condition = "Default")
    {
        _condition = (Conditions) Enum.Parse(typeof(Conditions), condition, true);
    }

    public string GetExperimentalCondition()
    {
        return _condition.ToString();
    }

    #endregion
}
