using System;
using System.Collections;
using System.Collections.Generic;
using Tobii.XR.GazeModifier;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    private AudioSource source;

    [Tooltip("If set to true, will turn the collider off after being hit.")]
    [SerializeField] private bool turnOffAfterPlay;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ManualController>())
        {
            source.Play();
            if (turnOffAfterPlay)
            {
                GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}
