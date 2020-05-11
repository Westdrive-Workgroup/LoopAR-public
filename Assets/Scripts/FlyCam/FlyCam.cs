using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// enables the camera to be moved by a mouse
/// </summary>

public class FlyCam : MonoBehaviour
{

    /*
	EXTENDED FLYCAM
		Desi Quintans (CowfaceGames.com), 17 August 2012.
		Based on FlyThrough.js by Slin (http://wiki.unity3d.com/index.php/FlyThrough), 17 May 2011.
 
	LICENSE
		Free as in speech, and free as in beer.
 
	FEATURES
		WASD/Arrows:    Movement
		          Q:    Climb
		          E:    Drop
                      Shift:    Move faster
                    Control:    Move slower
                        End:    Toggle cursor locking to screen (you can also press Ctrl+P to toggle play mode on and off).
	*/

    public float cameraSensitivity = 90;
    public float climbSpeed = 4;
    public float normalMoveSpeed = 10;
    public float slowMoveFactor = 0.25f;
    public float fastMoveFactor = 3;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    private bool smootRotation = false;
    private bool isCursorLocked = false;
    void Start()
    {
        Screen.lockCursor = true;
        isCursorLocked = true;
    }
    // updates the camera position
    void Update()
    {
        //moves the camera corresponding to the mouse movement as long as the curser is locked
        if (isCursorLocked)
        {
            rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
            rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
            rotationY = Mathf.Clamp(rotationY, -90, 90);
            //smoothes the rotation around the y-axis
            if (smootRotation)
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation,
                    Quaternion.AngleAxis(rotationX, Vector3.up), Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation,
                    transform.localRotation * Quaternion.AngleAxis(rotationY, Vector3.left), Time.deltaTime);
            }
            //moves the camera around the y-axis without smoothing
            else if (!smootRotation)
            {
                transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
                transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
            }
            // accelerates the camera 
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                transform.position += transform.forward * (normalMoveSpeed * fastMoveFactor) *
                                      Input.GetAxis("Vertical") *
                                      Time.deltaTime;
                transform.position += transform.right * (normalMoveSpeed * fastMoveFactor) *
                                      Input.GetAxis("Horizontal") *
                                      Time.deltaTime;

            }
            //slows down the camera
            else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                transform.position += transform.forward * (normalMoveSpeed * slowMoveFactor) *
                                      Input.GetAxis("Vertical") *
                                      Time.deltaTime;
                transform.position += transform.right * (normalMoveSpeed * slowMoveFactor) *
                                      Input.GetAxis("Horizontal") *
                                      Time.deltaTime;
            }
            // moves the camera
            else
            {
                transform.position += transform.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
                //transform.position += Vector3.Lerp(transform.position, transform.position + transform.forward * normalMoveSpeed * Input.GetAxis("Vertical"), Time.deltaTime);
                //transform.position += Vector3.Lerp(transform.position, transform.right * normalMoveSpeed * Input.GetAxis("Horizontal"), Time.deltaTime);
            }

            //Rises the camera up vertically
            if (Input.GetKey(KeyCode.Q))
            {
                transform.position += transform.up * climbSpeed * Time.deltaTime;
            }
            //lowers the camera vertically
            if (Input.GetKey(KeyCode.E))
            {
                transform.position -= transform.up * climbSpeed * Time.deltaTime;
            }
            //enables/disables the smoothrotation
            if (Input.GetKey(KeyCode.F1))
            {
                smootRotation = !smootRotation;
            }
        }
        if (Input.GetKeyDown(KeyCode.End))
        {
            //Cursor.lockState = (Cursor.lockState == CursorLockMode.None) ? CursorLockMode.Locked : CursorLockMode.None;
            Screen.lockCursor = (Screen.lockCursor == false) ? true : false;
            isCursorLocked = (isCursorLocked == false) ? true : false;
        }
    }
    //releases the Curser lock
    public void releaseCursor()
    {
        Screen.lockCursor = false;
    }
    //locks the cursor
    public void lockCursor ()
    {
        Screen.lockCursor = true;
        isCursorLocked = true;
    }
    //releases the cursor on destruction
    private void OnDestroy()
    {
        Screen.lockCursor = false;
    }
}