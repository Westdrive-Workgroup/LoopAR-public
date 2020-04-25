using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixationDot : MonoBehaviour
{
    public GameObject TargetPoint;
    private float fixationDuration;

    public float aimedFixationDuration;
    
    public delegate void OnHitTargetPoint(float duration);

    public event OnHitTargetPoint NotifyFixationTimeObservers;

    public delegate void OnLeftTargetPoint(bool leftTheTarget);
    public event OnLeftTargetPoint NotifyLeftTargetObservers;
    
    // Start is called before the first frame update
    void Start()
    {
        fixationDuration = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.forward, out hit, 50.0f))
        {
            if (hit.collider.gameObject == TargetPoint)
            {
                TargetPoint.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
            }

            fixationDuration += Time.deltaTime;
            

        }
        else
        {
            TargetPoint.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            fixationDuration = 0f;
            NotifyLeftTargetObservers?.Invoke(true);
        }

        NotifyFixationTimeObservers?.Invoke(aimedFixationDuration - fixationDuration);
    }
}
