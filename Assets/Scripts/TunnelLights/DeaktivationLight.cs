using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeaktivationLight : MonoBehaviour
{
    [SerializeField] private new Light light;

    [SerializeField] private float interval;
    [SerializeField] private float seconds;
    [SerializeField] private float minimumintensity;
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine("Lighting");
    }

    IEnumerator Lighting()
    {
        for (var ft = light.intensity; ft >= minimumintensity; ft -= interval)
        {
            light.intensity = ft;
            yield return new WaitForSeconds(seconds);
        }
    }
}

