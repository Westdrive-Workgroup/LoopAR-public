using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeValidationManager : MonoBehaviour
{
    public GameObject fixationPoint;
    
    private float participantHeight;

    public TextMesh HeadfixationText;
    
    public TextMesh FixingPoint;

    public TextMesh CounterText;

    private bool HeadIsFixated;
    // Start is called before the first frame update
    void Start()
    {
        participantHeight = EyetrackingManager.Instance.GetHmdTransform().transform.position.y;
        
        fixationPoint.transform.position= new Vector3(fixationPoint.transform.position.x,participantHeight,fixationPoint.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            EyetrackingManager.Instance.StartValidation();
        }
    }
}
