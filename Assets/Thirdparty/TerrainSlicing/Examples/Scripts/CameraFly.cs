using UnityEngine;
using System.Collections;

public class CameraFly : MonoBehaviour 
{
    [SerializeField]
    float speed = 30f;

    [SerializeField]
    string forward_backward_axis = "Vertical";

    [SerializeField]
    string left_right_axis = "Horizontal";
	// Update is called once per frame
	void Update () 
    {
        float for_back = Input.GetAxis(forward_backward_axis);
        float left_right = Input.GetAxis(left_right_axis);

        Vector3 for_back_vector = transform.forward * for_back;

        Vector3 left_right_vector = transform.right * left_right;

        transform.position += (for_back_vector + left_right_vector) * speed * Time.deltaTime;
	}
}
