using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudLite_TestDrive : MonoBehaviour
{
    private List<GameObject> _eventObjectsToMark;
    public GameObject highlightSymbol;
    public GameObject EventAnzeigen;
    public GameObject NonEventAnzeigen;
    


    private List<GameObject> _highlightedObjects;

    private GameObject _objectToHighlight;

    private void Awake()
    {
        _highlightedObjects = new List<GameObject>();
    }

    private void Start()
    {
        Debug.Log("Start");
       // _highlightedObjects = new List<GameObject>();
       NonEventAnzeigen.SetActive(true); 
       EventAnzeigen.SetActive(false);
    }

    public void ActivateHUD(GameObject testAccidentObject)
    {
        /*if (_objectToHighlight != null)
        {
            _objectToHighlight = null;
        }*/
        
        List<GameObject> ObjectToMark = new List<GameObject>();
        ObjectToMark.Add(testAccidentObject);
        ActivateHUD(ObjectToMark);
        
        
        /*_objectToHighlight = testAccidentObject;
        MarkObject(_objectToHighlight);*/
    }

    public void ActivateHUD(List<GameObject> testAccidentSubjects)
    {
        _eventObjectsToMark = new List<GameObject>(testAccidentSubjects);
        MarkObjects();
    }
    public void DriverAlert()
    {
        Debug.Log("DriveAlert");
        EventAnzeigen.SetActive(true);
        NonEventAnzeigen.SetActive(false);
    }

    public void DeactivateHUD()
    {
        // _objectToHighlight = null;
        _eventObjectsToMark.Clear();
        foreach (var highlightedObject in _highlightedObjects)
        {
            // Destroy(highlightedObject);
        }
        NonEventAnzeigen.SetActive(true);
        EventAnzeigen.SetActive(false);
        
        _highlightedObjects.Clear();
    }


    private void MarkObjects()
    {
       Debug.Log("mark objects");
       foreach (var eventObject in _eventObjectsToMark)
       {
           Debug.Log(eventObject.gameObject.transform.name);
           GameObject clone = Instantiate(highlightSymbol, eventObject.transform.position, eventObject.transform.rotation, eventObject.transform );
           //clone.transform.localPosition = Vector3.RotateTowards(transform.forward, Camera.main.transform.position , 0 , 0.0f);
           _highlightedObjects.Add(clone); // just to 
       }
    }

    private void MarkObject(GameObject testObject)
    {
        GameObject clone= Instantiate(highlightSymbol, testObject.transform);
        clone.transform.localPosition = Vector3.up;
    }
}