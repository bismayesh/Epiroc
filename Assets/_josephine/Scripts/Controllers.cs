using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controllers : MonoBehaviour
{
    public Transform knobAngle;
    public Renderer lightRenderer;
    public TaskDrive taskDrive;
    public TaskDrill taskDrill;
    public TaskLights taskLights;
    public bool isActive = false;

    public Material greenMat;
    public Material redMat;

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
            lightRenderer.material = redMat;
        }
        else
        {
            isActive = true;
            lightRenderer.material = greenMat;
        }

        //taskDrive.ActivateButton(this.gameObject);
    }

    public void SwitchTurned()
    {
        localRotation = knobAngle.localRotation.eulerAngles.y;
        localRotation = (Mathf.Abs(localRotation))%180;

        if (localRotation == 0)
        {
            isActive = false;
            lightRenderer.material = redMat;
        }
        else
        {
            isActive = true;
            lightRenderer.material = greenMat;
        }
        //taskDrive.StartEngine(this.gameObject, isActive);
    }
}
