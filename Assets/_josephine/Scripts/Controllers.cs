using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controllers : MonoBehaviour
{
    public Transform switchTransform;
    public Renderer lightRenderer;
    public bool isActive = false;

    public Material MaterialOn;
    public Material MaterialOff;

    float localRotation;
    bool initialSwitch = true;

    public void MenuButtonPressed()
    {
        if (isActive)
        {
            isActive = false;
            TaskInstructions.instance.TaskCheck(this.gameObject, false);
        }
        else
        {
            isActive = true;
            TaskInstructions.instance.TaskCheck(this.gameObject, true);
        }

        if (!SupportLevels.instance.instructionsFinnished)
        {
            SupportLevels.instance.SupportInstructions(SupportMood.Introduction, gameObject);
        }
    }

    public void ButtonPressed()
    {
        
        if (isActive)
        {
            isActive = false;
            lightRenderer.material = MaterialOff;
            TaskDrive.instance.TaskCheck(this.gameObject, false);
            TaskDrill.instance.TaskCheck(this.gameObject, false);
            TaskTorch.instance.TaskCheck(this.gameObject, false);
        }
        else
        {
            isActive = true;
            lightRenderer.material = MaterialOn;
            TaskDrive.instance.TaskCheck(this.gameObject, true);
            TaskDrill.instance.TaskCheck(this.gameObject, true);
            TaskTorch.instance.TaskCheck(this.gameObject, true);
        }

        SupportLevels.instance.SupportInstructions( SupportMood.Old, gameObject);
    }

    public void TurnSwitch()
    {
        localRotation = switchTransform.localRotation.eulerAngles.y;
        localRotation = (Mathf.Abs(localRotation))%180;

        if (localRotation == 0)
        {
            isActive = false;
            lightRenderer.material = MaterialOff;
            TaskDrive.instance.TaskCheck(this.gameObject, false);
        }
        else
        {
            isActive = true;
            lightRenderer.material = MaterialOn;
            TaskDrive.instance.TaskCheck(this.gameObject, true);
        }

        SupportLevels.instance.SupportInstructions( SupportMood.Old, gameObject);
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
            TaskDrive.instance.TaskCheck(this.gameObject, false);
            TaskTorch.instance.TaskCheck(this.gameObject, false);
        }
        else
        {
            isActive = true;
            lightRenderer.material = MaterialOn;
            TaskDrive.instance.TaskCheck(this.gameObject, true);
            TaskTorch.instance.TaskCheck(this.gameObject, true);
        }

        SupportLevels.instance.SupportInstructions( SupportMood.Old, gameObject);
    }
}
