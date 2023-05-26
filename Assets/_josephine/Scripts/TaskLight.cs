using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskLight : MonoBehaviour
{
    public GameObject activateButton;
    public GameObject fucusSlider;
    public GameObject intensitySlider;

    [SerializeField]
    public bool activateButtonOn = false;
    [SerializeField]
    public bool fucusSliderOn = false;
    [SerializeField]
    public bool intensitySliderOn = false;
    [SerializeField]
    bool operatesLight = false;

    public TrainingState currentTraining;
    public MachineController machineController;

    bool holdingJoystick = false;
    bool failureRecorded = false;

    public int neededTrolls = 5;
    int currentCheckpoint = 0;

    public bool LightMood
    {
        get { return activateButtonOn; }
        private set { activateButtonOn = value; }
    }

    public bool HoldingJoystick
    {
        get { return holdingJoystick; }
        set
        {
            holdingJoystick = value;
        }
    }

    private void Update()
    {
        if (!holdingJoystick)
        {
            failureRecorded = false;
        }
    }
    private void Start()
    {
        currentTraining = GameObject.FindGameObjectWithTag("Training").GetComponent<TrainingState>();
        machineController = GameObject.Find("Machine").GetComponent<MachineController>();
    }

    public void ActivateButton(GameObject thisObject)
    {
        ButtonSwitch(thisObject, activateButton, ref activateButtonOn);
        ButtonSwitch(thisObject, fucusSlider, ref fucusSliderOn);
        ButtonSwitch(thisObject, intensitySlider, ref intensitySliderOn);
    }

    private void ButtonSwitch(GameObject thisObject, GameObject button, ref bool buttonIsActive)
    {

        if (thisObject == button)
        {
            if (!buttonIsActive)
            {
                buttonIsActive = true;

                if (button == activateButton)
                {
                    Debug.Log("activate lights on");
                }
                else if (button == fucusSlider)
                {
                    Debug.Log("lights focosed");
                    //Focus lights method
                    //machineController.ActivateEngine();
                }
                else if (button == intensitySlider)
                {
                    Debug.Log("light intensity up");
                    //Light intensity method
                    //machineController.ReleaseBrakes();
                }
            }
            else
            {
                buttonIsActive = false;

                if (button == this.activateButton)
                {
                    Debug.Log("activate lights off");
                }
                else if (button == fucusSlider)
                {
                    Debug.Log("lights unfocosed");
                    //Focus lights method
                    //machineController.ActivateEngine();
                }
                else if (button == intensitySlider)
                {
                    Debug.Log("light intensity down");
                    //Light intensity method
                    //machineController.ReleaseBrakes();
                }
            }
        }
    }

    public void Torch(Vector2 force)
    {
        if (holdingJoystick)
        {
            if (activateButtonOn && fucusSliderOn && intensitySliderOn)
            {
                Debug.Log("Operating lights");
                operatesLight = true;
                //Operate lights method
                //machineController.ChangeMovementForce(force);
            }
            else
            {
                if (!failureRecorded)
                {
                    failureRecorded = true;
                    TaskFailure();
                }
            }
        }
        else
        {
            operatesLight = false;
        }
    }

    void TaskFailure()
    {
        Debug.Log("Light task fail!");
        currentTraining.UpdateTrainingFailures();
        //Show ghost animation
    }
}
