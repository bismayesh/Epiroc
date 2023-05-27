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

    public TrainingState currentTraining;
    public TaskDrive taskDrive;
    public TaskDrill taskDrill;
    public TaskLight taskLight;
    public SupportLevels supportLevels;

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

    enum JoystickMood { Drive, Drill, Light}
    JoystickMood joystickMood = JoystickMood.Drive;

    private void Start()
    {
        currentTraining = GameObject.FindGameObjectWithTag("Training").GetComponent<TrainingState>();
        taskDrive = GameObject.Find("Drive").GetComponent<TaskDrive>();
        taskDrill = GameObject.Find("Drill").GetComponent<TaskDrill>();
        taskLight = GameObject.Find("Torch").GetComponent<TaskLight>();
        supportLevels = GameObject.Find("SupportLevels").GetComponent<SupportLevels>();
    }

    void FixedUpdate()
    {
        primaryButtonValue = primaryButtonAction.action.ReadValue<float>();
        secondaryButtonValue = secondaryButtonAction.action.ReadValue<float>();
        gripButtonValue = gripButtonAction.action.ReadValue<float>();

        if (gripButtonValue == 0f)
        {
            holdingJoystick = false;
            taskDrive.HoldingJoystick = false;
            taskDrill.HoldingJoystick = false;
            taskLight.HoldingJoystick = false;
            supportLevels.holdingJoystick = false;
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
            taskDrive.HoldingJoystick = true;
            taskDrill.HoldingJoystick = true;
            taskLight.HoldingJoystick = true;
            supportLevels.holdingJoystick = true;
        }
    }

    public void ReadJoystickInput(Vector2 value)
    {
        if (taskDrive.DriveMood)
        {
            taskDrive.Drive(value, this.gameObject);
        }
        else if (taskDrill.DrillMood)
        {
            taskDrill.Drill(value);
        }
        else if (taskLight.LightMood)
        {
            taskLight.Torch(value, this.gameObject);
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
            taskDrill.ActivateButton(topButton);
            taskLight.ActivateButton(topButton);
            supportLevels.UserSupport(topButton);
        }
    }

    IEnumerator ButtonPressDelay()
    {
        yield return new WaitForSeconds(0.1f);
        buttonJustPressed = false;
    }
}
