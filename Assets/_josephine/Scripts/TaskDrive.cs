using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TaskDrive : MonoBehaviour
{
    //Only for testing movement on prototype
    public MoveForward moveForward;
    //Only for testing movement on prototype

    public GameObject activationSwitch;
    public GameObject allocateButton;
    public GameObject breakButton;

    [SerializeField]
    bool activationSwitchOn = false;
    [SerializeField]
    bool allocateButtonOn = false;
    [SerializeField]
    bool breakButtonOn = false;
    [SerializeField]
    bool driving = false;
    [SerializeField]
    bool holdingJoystick = false;
    [SerializeField]
    bool failureRecorded = false;

    public TrainingState currentTraining;
    public MachineController machineController;
    public TaskDrill taskDrill;

    public int neededCheckpoints = 5;
    int currentCheckpoint = 0;

    public bool DriveMood
    {
        get { return activationSwitchOn; }
        private set { activationSwitchOn = value; }
    }

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
        taskDrill = GameObject.Find("Drill").GetComponent<TaskDrill>();
    }

    private void Update()
    {
        if (!holdingJoystick)
        {
            failureRecorded = false;
        }
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

    //Take away input parameter thisobject when stop using method for testdrive
    public void Drive(Vector2 force, GameObject thisObject)
    {
        if (holdingJoystick)
        {
            if (taskDrill.MachineStabalized && !failureRecorded)
            {
                failureRecorded = true;
                Debug.Log("Serius damage inflicted to vehicle");
                currentTraining.MachineDamage += 5;
                TaskFailure();
                return;
            }

            if (activationSwitchOn && allocateButtonOn)
            {
                if (breakButtonOn)
                {
                    Debug.Log("Driving");
                    driving = true;

                    //Only for testing movement on prototype
                    moveForward.DriveTest(force, thisObject);
                    //Only for testing movement on prototype

                    machineController.ChangeMovementForce(force);
                }
            }
            else
            {
                if (!failureRecorded)
                {
                    failureRecorded = true;
                    Debug.Log(failureRecorded);
                    TaskFailure();
                }
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
