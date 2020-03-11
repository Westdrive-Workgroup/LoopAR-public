using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CameraFollow : MonoBehaviour
{
    public GameObject objectToFollow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        this.transform.position = objectToFollow.transform.position;
    }
    // Update is called once per frame
    
}