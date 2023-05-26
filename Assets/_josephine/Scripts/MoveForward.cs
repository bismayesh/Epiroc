using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speedForward = 15f;
    public float speedRotate = 100f;

    private float forceForward;
    private float forceRotation;
    
    void Update()
    {
        transform.Translate(Vector3.forward * speedForward * forceForward * Time.deltaTime);
        transform.Rotate(Vector3.up, speedRotate * forceRotation * Time.deltaTime);
    }

    public void DriveTest(Vector2 value, GameObject gameObject)
    {
        if (!gameObject.CompareTag("LeftJoystick"))
        {
            

            if (Mathf.Abs(value.y) > 0.2f)
            {
                forceForward = value.y;
            }
            else
            {
                forceForward = 0;
            }
        }
        else
        {
            if (Mathf.Abs(value.x) > 0.2f)
            {
                forceRotation = value.x;
            }
            else
            {
                forceRotation = 0;
            }
        }
    }
}
