using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDLite : MonoBehaviour
{
    private List<GameObject> EventObjectsToMark;

    public GameObject HighlightSymbol;
    
    
    public void ActivateHUD(GameObject testAccidentSubject)
    {
        List<GameObject> ObjectToMark = new List<GameObject>();
        ObjectToMark.Add(testAccidentSubject);
        ActivateHUD(ObjectToMark);
    }

    public void ActivateHUD(List<GameObject> testAccidentSubjects)
    {
        MarkObjects();
    }

    public void DeactivateHUD()
    {
        EventObjectsToMark.Clear();
    }


    private void MarkObjects()
    {
        foreach (var eventObject in EventObjectsToMark)
        {
            HighlightSymbol.transform.SetParent(eventObject.transform);
        }
    }
    
    
}
