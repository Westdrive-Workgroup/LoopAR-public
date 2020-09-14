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
        FullLoopAR,
        BaseCondition,
        HUDOnly,
        AudioOnly,
        FullLoopARDefault
    }

    private Conditions _condition;
    
    #region PrivateMethods
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void DriverAlertCondition()
    {
        switch (_condition)
        {
            case Conditions.FullLoopAR:
                SceneLoadingHandler.Instance.GetParticipantsCar().GetComponentInChildren<HUD_Advance>().DriverAlert();
                break;
            case Conditions.HUDOnly:
                SceneLoadingHandler.Instance.GetParticipantsCar().GetComponentInChildren<HUD_Advance>().DrawOnlyTriangle();    // todo test
                break;
            case Conditions.AudioOnly:
                SceneLoadingHandler.Instance.GetParticipantsCar().GetComponentInChildren<HUD_Advance>().OnlyAudioHUD();    // todo test
                break;
            case Conditions.BaseCondition:
                break;
            case Conditions.FullLoopARDefault:
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
            case Conditions.FullLoopARDefault:
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
                SceneLoadingHandler.Instance.GetParticipantsCar().GetComponentInChildren<HUD_Advance>().DeactivateHUD(false);
                break;
            case Conditions.AudioOnly:
                if (_playTakeOverRequest)
                    SceneLoadingHandler.Instance.GetParticipantsCar().GetComponentInChildren<HUD_Advance>().OnlyAudioHUDTOR(); // todo test
                break;
            case Conditions.BaseCondition:
                break;
            case Conditions.FullLoopARDefault:
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
    
    public void SetExperimentalCondition(string condition = "FullLoopARDefault")
    {
        _condition = (Conditions) Enum.Parse(typeof(Conditions), condition, true);
    }

    public string GetExperimentalCondition()
    {
        return _condition.ToString();
    }

    #endregion
}
