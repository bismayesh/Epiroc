using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoyStick : MonoBehaviour
{
    public InputActionProperty primaryButtonAction;
    public InputActionProperty secondaryButtonAction;
    public InputActionProperty gripButtonAction;

    public TaskDrive taskDrive;

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

    private void Start()
    {
        taskDrive = GameObject.Find("Drive").GetComponent<TaskDrive>();
    }

    void FixedUpdate()
    {
        primaryButtonValue = primaryButtonAction.action.ReadValue<float>();
        secondaryButtonValue = secondaryButtonAction.action.ReadValue<float>();
        gripButtonValue = gripButtonAction.action.ReadValue<float>();
    }

    private void OnTriggerStay(Collider other)
    {
        HoldingJoystick(other);
        TopButtonPressed(primaryButton, primaryButtonValue, ref primaryIsActive, light1);
        TopButtonPressed(secondaryButton, secondaryButtonValue, ref secondaryIsActive, light2);
    }

    private void HoldingJoystick(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && gripButtonValue != 0f)
        {
            holdingJoystick = true;
            taskDrive.HoldingJoystick = true;
        }
        else
        {
            holdingJoystick = false;
            taskDrive.HoldingJoystick = false;
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

            taskDrive.ActivateButton(topButton);
        }
    }

    IEnumerator ButtonPressDelay()
    {
        yield return new WaitForSeconds(0.1f);
        buttonJustPressed = false;
    }
}
