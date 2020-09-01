using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.AI;

public class blinker_hu : MonoBehaviour
{
    
    public Material mat;
    public Color color1;
    public Color color2;

    public float intensity;
    //private Color orange = new Color(255f, 165f, 0.0f, a:1.0f);
    public int counter;

    void Start()
    {
        StartCoroutine("Flicker");
    }

    

    IEnumerator Flicker()
    {
        while (true)
        {
            counter++;

            if (counter % 2 == 0)
            {
                mat.SetColor("_EmissionColor",  color1 * intensity);
            } else
            {
                mat.SetColor("_EmissionColor", color2 * 1.1f);;
            }
            yield return new WaitForSeconds(0.5f);
        }
    } 
}
