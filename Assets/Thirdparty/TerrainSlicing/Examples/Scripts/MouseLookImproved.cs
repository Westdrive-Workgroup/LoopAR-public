using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// This Improved MouseLook script is virtually the same as the MouseLook.cs version found in the Character Controller
/// package of Standard Assets, with one small improvement. A new method (UpdateYRotationWithCurrentValue) has been added which updates
/// the y rotation stored by the script so that it uses the actual rotation.
/// 
/// Previously, if you tried to set the rotation of the object this
/// script was on manually (for instance, if trying to set the object to its state from a previous game session), that manual
/// rotation would be overriden and not used. Whenever you try to change the rotation of the object, you should call 
/// UpdateYRotationWithCurrentValue.
/// 
/// Note that this method is called in Start automatically, so that the script will use the rotation set in the inspector or from
/// another scripts Awake method.
[AddComponentMenu("Dynamic Loading Kit/CharacterControl/Mouse Look Improved")]
public class MouseLookImproved : MonoBehaviour
{

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;

    void Update()
    {
        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }

    void Start()
    {
        UpdateYRotationWithCurrentValue();
        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
    }

    public void UpdateYRotationWithCurrentValue()
    {
        rotationY = -transform.localEulerAngles.x;
    }
}