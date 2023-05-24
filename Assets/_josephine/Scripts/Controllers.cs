using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controllers : MonoBehaviour
{
    public Transform switchTransform;
    public Renderer lightRenderer;
    public TaskDrive taskDrive;
    public TaskDrill taskDrill;
    public TaskLights taskLights;
    public bool isActive = false;

    public Material MaterialOn;
    public Material MaterialOff;

    private float localRotation;

    private void Start()
    {
        taskDrive = GameObject.Find("Drive").GetComponent<TaskDrive>();
        taskDrive = GameObject.Find("Drill").GetComponent<TaskDrive>();
        taskDrive = GameObject.Find("Lights").GetComponent<TaskDrive>();
    }

    public void ButtonPressed()
    {
        
        if (isActive)
        {
            isActive = false;
            lightRenderer.material = MaterialOff;
        }
        else
        {
            isActive = true;
            lightRenderer.material = MaterialOn;
        }

        //taskDrive.ActivateButton(this.gameObject);
    }

    public void TurnSwitch()
    {
        localRotation = switchTransform.localRotation.eulerAngles.y;
        localRotation = (Mathf.Abs(localRotation))%180;

        if (localRotation == 0)
        {
            isActive = false;
            lightRenderer.material = MaterialOff;
        }
        else
        {
            isActive = true;
            lightRenderer.material = MaterialOn;
        }
        //taskDrive.StartEngine(this.gameObject, isActive);
    }

    public void Switch()
    {
        float angle = switchTransform.localEulerAngles.x;
        angle = (angle > 180) ? angle - 360 : angle;

        if (angle > 0)
        {
            isActive = false;
            lightRenderer.material = MaterialOff;
        }
        else
        {
            isActive = true;
            lightRenderer.material = MaterialOn;
        }
    }
}
