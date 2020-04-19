using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDLite : MonoBehaviour
{
    private List<GameObject> EventObjectsToMark;

    public GameObject HighlightSymbol;

    private List<GameObject> _highlightedObjects;

    private void Start()
    {
        _highlightedObjects = new List<GameObject>();
    }

    public void ActivateHUD(GameObject testAccidentSubject)
    {
        List<GameObject> ObjectToMark = new List<GameObject>();
        ObjectToMark.Add(testAccidentSubject);
        ActivateHUD(ObjectToMark);
    }

    public void ActivateHUD(List<GameObject> testAccidentSubjects)
    {
        EventObjectsToMark = testAccidentSubjects;
        MarkObjects();
    }

    public void DeactivateHUD()
    {
        EventObjectsToMark.Clear();
        foreach (var highlightedObject in _highlightedObjects)
        {
            Destroy(highlightedObject);
        }
        
        _highlightedObjects.Clear();
    }


    private void MarkObjects()
    {
        Debug.Log("mark objects");
        foreach (var eventObject in EventObjectsToMark)
        {
            GameObject clone= Instantiate(HighlightSymbol, eventObject.transform);
            clone.transform.localPosition = Vector3.up;
            _highlightedObjects.Add(clone); // just to 
        }
    }
    
    
}
