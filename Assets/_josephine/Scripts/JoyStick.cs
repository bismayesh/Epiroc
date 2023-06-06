using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public enum JoystickMood
{ 
  Drive, 
  Drill,
  Light
}

public class JoyStick : MonoBehaviour
{
    public InputActionProperty primaryButtonAction;
    public InputActionProperty secondaryButtonAction;
    public InputActionProperty gripButtonAction;

    public GameObject primaryButton;
    public GameObject secondaryButton;
    public Renderer light1;
    public Renderer light2;
    public Material MaterialOn;
    public Material MaterialOff;

    public bool holdingJoystick = false;
    public bool primaryIsActive = false;
    public bool secondaryIsActive = false;
    public bool buttonJustPressed  = false;

    float primaryButtonValue;
    float secondaryButtonValue;
    float gripButtonValue;

    
    //JoystickMood joystickMood = JoystickMood.Drive;

    void FixedUpdate()
    {
        primaryButtonValue = primaryButtonAction.action.ReadValue<float>();
        secondaryButtonValue = secondaryButtonAction.action.ReadValue<float>();
        gripButtonValue = gripButtonAction.action.ReadValue<float>();

        if (gripButtonValue == 0f)
        {
            holdingJoystick = false;
            TaskDrive.instance.HoldingJoystick = false;
            TaskDrill.instance.HoldingJoystick = false;
            TaskTorch.instance.HoldingJoystick = false;
            SupportLevels.instance.holdingJoystick = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        HoldingJoystick(other);
        TopButtonPressed(primaryButton, primaryButtonValue, ref primaryIsActive, light1);
        TopButtonPressed(secondaryButton, secondaryButtonValue, ref secondaryIsActive, light2);
    }

    private void HoldingJoystick(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && gripButtonValue > 0.5f)
        {
            holdingJoystick = true;
            TaskDrive.instance.HoldingJoystick = true;
            TaskDrill.instance.HoldingJoystick = true;
            TaskTorch.instance.HoldingJoystick = true;
            SupportLevels.instance.holdingJoystick = true;
        }
    }

    public void ReadJoystickInput(Vector2 value)
    {
        if (TaskDrive.instance.DriveMood)
        {
            TaskDrive.instance.Drive(value, this.gameObject);
        }
        else if (TaskDrill.instance.DrillMood)
        {
            TaskDrill.instance.Drill(value);
        }
        else if (TaskTorch.instance.LightMood)
        {
            TaskTorch.instance.Torch(value, this.gameObject);
        }
    }

    private void TopButtonPressed(GameObject topButton, float topButtonValue, ref bool isActive, Renderer light)
    {
        if (holdingJoystick && !buttonJustPressed && topButtonValue != 0f)
        {
            buttonJustPressed = true;
            StartCoroutine(ButtonPressDelay());

            if (isActive)
            {
                isActive = false;
                light.material = MaterialOff;
            }
            else
            {
                isActive = true;
                light.material = MaterialOn;
            }

            TaskDrive.instance.TaskCheck(topButton);
            TaskDrill.instance.TaskCheck(topButton);
            TaskTorch.instance.TaskCheck(topButton);
            SupportLevels.instance.UserSupport(topButton);
        }
    }

    IEnumerator ButtonPressDelay()
    {
        yield return new WaitForSeconds(0.1f);
        buttonJustPressed = false;
    }
}
