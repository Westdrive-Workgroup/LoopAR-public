using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    public bool control = true;

    // Update is called once per frame
    void Update()
    {
        if (control == true)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(speed * transform.forward);
            }

            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(speed * -1 * transform.forward);
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(speed * -1 * transform.right);
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(speed * transform.right);
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (control == false)
            {
                control = true;
            }
        }
    }
}
