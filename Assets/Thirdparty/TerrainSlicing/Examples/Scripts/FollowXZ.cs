using UnityEngine;
using System.Collections;

public class FollowXZ : MonoBehaviour 
{
    [SerializeField]
    Transform target;

    Transform _transform;
	// Use this for initialization
	void Start () 
    {
        _transform = transform;
        MatchTargetsXZPosition();
	}
	
	void Update () 
    {
        MatchTargetsXZPosition();
	}

    void MatchTargetsXZPosition()
    {
        _transform.position = new Vector3(target.position.x, _transform.position.y, target.position.z);
    }
}
