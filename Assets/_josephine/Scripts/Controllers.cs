using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controllers : MonoBehaviour
{
    public Transform switchTransform;
    public Renderer lightRenderer;
    public TaskDrive taskDrive;
    public TaskDrill taskDrill;
    public TaskLight taskLights;
    public bool isActive = false;

    public Material MaterialOn;
    public Material MaterialOff;

    float localRotation;
    bool initialSwitch = true;

    private void Start()
    {
        taskDrive = GameObject.Find("Drive").GetComponent<TaskDrive>();
        taskDrill = GameObject.Find("Drill").GetComponent<TaskDrill>();
        taskLights = GameObject.Find("Torch").GetComponent<TaskLight>();
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

        taskDrive.ActivateButton(this.gameObject);
        taskLights.ActivateButton(this.gameObject);
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

        taskDrive.ActivateButton(this.gameObject);
    }

    public void Switch()
    {
        if (initialSwitch)
        {
            initialSwitch = false;
            return;
        }

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

        taskDrive.ActivateButton(this.gameObject);
        taskLights.ActivateButton(this.gameObject);
    }
}
