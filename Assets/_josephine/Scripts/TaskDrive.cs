using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TaskDrive : Task
{
    //[Header("Events")]
    //public UnityEvent onDriving;

    public AudioClip driveAudio;
    public List<SingleTask> taskControl = new List<SingleTask>();
    GameObject lastCheckpoint = null;

    //Singelton
    public static TaskDrive instance;

    private void Awake()
    {
        instance = this;
    }

    public bool DriveMood
    {
        get { return taskControl[2].isOn; }
        private set { taskControl[2].isOn = value; }
    }

    public void TaskCheck(GameObject thisObject)
    {
        if (taskControl[0].TaskCheck(thisObject))
            ActivationSwitch();
        if (taskControl[1].TaskCheck(thisObject))
            AllocateButton();
        if (taskControl[2].TaskCheck(thisObject))
            BreakButton();
    }

    private void ActivationSwitch()
    {
        if (taskControl[0].isOn)
        {
            machineController.ActivateEngine();
        }
        else
        {

        }
    }

    private void AllocateButton()
    {
        if (taskControl[1].isOn)
        {
            machineController.ReleaseBrakes();
        }
        else
        {

        }
    }

    private void BreakButton()
    {
        if (taskControl[2].isOn)
        {
            machineController.ReleaseBrakes();
        }
        else
        {
            SetAudio(driveAudio, false, false);
        }
    }

    public void Drive(Vector2 force, GameObject thisObject)
    {
        if (!holdingJoystick)
        {
            SetAudio(driveAudio, false, false);
            return;
        }


        if (TaskDrill.instance.MachineStabalized)
        {
            DriveFailure(5);
            return;
        }

        if (taskControl[0].isOn && taskControl[1].isOn)
        {
            if (taskControl[2].isOn)
            {
                SetAudio(driveAudio, true, false);
                


                if (!thisObject.CompareTag("LeftJoystick"))
                {
                    force = force * -1;
                    machineController.ChangeMovementForce(force);
                }
                if (thisObject.CompareTag("LeftJoystick"))
                {
                    machineController.ChangeRotationForce(force);
                }
            }
        }
        else
        {
            DriveFailure();
        }
    }

    public void DriveProgress(GameObject thisCheckpoint)
    {
        if (thisCheckpoint != lastCheckpoint)
        {
            lastCheckpoint = thisCheckpoint;
            currentIteration++;
            TrainingState.instance.UpdateTaskDriveProgress(neededIterations, currentIteration);
        }
    }

    void DriveFailure(int damage = 0)
    {
        if (failRecorded)
            return;

        failRecorded = true;
        TaskFailure(damage);
        TrainingState.instance.UpdateDriveFailure();
    }
}
