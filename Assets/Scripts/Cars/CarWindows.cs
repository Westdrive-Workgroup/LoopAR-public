using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWindows : MonoBehaviour
{
    public GameObject insideWindows;
    public float GetInsideWindowsAlphaChannel()
    {
        return insideWindows.GetComponent<Material>().color.a;
    }
    
    public void SetInsideWindowsAlphaChannel(float num)
    {
        Color color = insideWindows.GetComponent<Renderer>().material.color;
        color.a = num;
        insideWindows.GetComponent<Renderer>().material.color = color;
    }

    public GameObject GetInsideWindows()
    {
        return insideWindows;
    }
}
