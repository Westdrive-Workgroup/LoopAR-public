using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatingLight : MonoBehaviour
{
    [SerializeField] private new Light light;

    [SerializeField] private float interval;
    [SerializeField] private float seconds;
    [SerializeField] private float maximumIntensity;
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine("Lighting");
    }

    IEnumerator Lighting()
    {
        for (var ft = light.intensity; ft <= maximumIntensity; ft += interval)
        {
            light.intensity = ft;
            yield return new WaitForSeconds(seconds);
        }
    }
}