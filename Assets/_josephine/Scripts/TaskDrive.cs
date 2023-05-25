using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskDrive : MonoBehaviour
{
    public GameObject activationSwitch;
    public GameObject allocateButton;
    public GameObject breakButton;
    //public GameObject activateEngine;

    public bool activationSwitchOn = false;
    public bool allocateButtonOn = false;
    public bool breakButtonOn = false;
    public bool driving = false;
    private bool holdingJoystick = false;

    public TrainingState currentTraining;
    public MachineController machineController;

    public int neededCheckpoints = 5;
    int currentCheckpoint = 0;

    public int CurrentCheckpoint
    {
        get { return currentCheckpoint; }
        set 
        { 
            currentCheckpoint = value;
            currentTraining.UpdateTaskDriveProgress( neededCheckpoints, currentCheckpoint);
        }
    }

    public bool HoldingJoystick
    {
        get { return holdingJoystick; }
        set 
        {
            holdingJoystick = value;
        }
    }

    private void Start()
    {
        currentTraining = GameObject.FindGameObjectWithTag("Training").GetComponent<TrainingState>();
        machineController = GameObject.Find("Machine").GetComponent<MachineController>();
    }


    public void ActivateButton(GameObject thisObject)
    {
        ButtonSwitch(thisObject, activationSwitch, ref activationSwitchOn);
        ButtonSwitch(thisObject, allocateButton, ref allocateButtonOn);
        ButtonSwitch(thisObject, breakButton, ref breakButtonOn);
    }

    private void ButtonSwitch(GameObject thisObject, GameObject button, ref bool activateButton)
    {

        if (thisObject == button)
        {
            if (!activateButton)
            {
                activateButton = true;

                if (button == activationSwitch)
                {
                    machineController.ActivateEngine();
                }
                else if (button == breakButton)
                {
                    machineController.ReleaseBrakes();
                }
            }
            else
            { 
                activateButton = false;
            }
        }
    }

    public void Drive(Vector2 force)
    {
        if (holdingJoystick)
        {
            if (activationSwitchOn && allocateButtonOn)
            {
                if(breakButtonOn)
                {
                    Debug.Log("Driving");
                    driving = true;
                    machineController.ChangeMovementForce(force);
                }
            }
            else
            {
                driving = false;
                TaskFailure();
            }
        }
        else
        {
            driving = false;
        }
    }

    void TaskFailure()
    {
        Debug.Log("Drive task fail!");
        currentTraining.UpdateTrainingFailures();
        //Show ghost animation
    }
}
