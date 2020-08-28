using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.AI;

public class blinker_hu : MonoBehaviour
{
    
    public Material mat;
    private Color orange = new Color(255f, 165f, 0.0f, a:1.0f);
    private int counter;

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
                mat.SetColor("_EmissionColor",  orange * 0.05f);
            } else
            {
                mat.SetColor("_EmissionColor", Color.white * 1.1f);;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
