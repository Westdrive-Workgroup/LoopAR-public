using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ChaseCam : MonoBehaviour
{
    public GameObject objectToFollow;
    [Range(0f, 10f)] public float damping;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    
    
    
    private void LateUpdate()
    {
        this.transform.position = objectToFollow.transform.position;
        
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, objectToFollow.transform.rotation,
            Time.deltaTime * damping);
    }
    // Update is called once per frame
    
}