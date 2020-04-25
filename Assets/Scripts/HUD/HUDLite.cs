using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDLite : MonoBehaviour
{
    private List<GameObject> _eventObjectsToMark;

    public GameObject highlightSymbol;

    private List<GameObject> _highlightedObjects;

    private GameObject _objectToHighlight;
        
        
    private void Start()
    {
        _highlightedObjects = new List<GameObject>();
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
        _eventObjectsToMark = testAccidentSubjects;
        MarkObjects();
    }

    public void DeactivateHUD()
    {
        // _objectToHighlight = null;
        _eventObjectsToMark.Clear();
        foreach (var highlightedObject in _highlightedObjects)
        {
            // Destroy(highlightedObject);
        }
        
        _highlightedObjects.Clear();
    }


    private void MarkObjects()
    {
        // Debug.Log("mark objects");
        foreach (var eventObject in _eventObjectsToMark)
        {
            GameObject clone= Instantiate(highlightSymbol, eventObject.transform);
            clone.transform.localPosition = Vector3.up;
            _highlightedObjects.Add(clone); // just to 
        }
    }

    private void MarkObject(GameObject testObject)
    {
        GameObject clone= Instantiate(highlightSymbol, testObject.transform);
        clone.transform.localPosition = Vector3.up;
    }
}
