using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TaskDrive : Task
{
    //[Header("Events")]
    //public UnityEvent onDriving;

    public AudioSource engineSound;
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
        get { return taskControl[2].IsOn; }
    }

    public void TaskCheck(GameObject thisObject, bool setOn)
    {
        if (taskControl[0].TaskCheck(thisObject, setOn))
            ActivationSwitch();
        if (taskControl[1].TaskCheck(thisObject, setOn))
            AllocateButton();
        if (taskControl[2].TaskCheck(thisObject, setOn))
            BreakButton();
    }

    private void ActivationSwitch()
    {
        if (taskControl[0].IsOn)
        {
            engineSound.Play();
            machineController.ActivateEngine();
        }
        else
        {
            engineSound.Stop();
        }
    }

    private void AllocateButton()
    {
        if (taskControl[1].IsOn)
        {
            machineController.ReleaseBrakes();
        }
        else
        {

        }
    }

    private void BreakButton()
    {
        if (taskControl[2].IsOn)
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


        if (taskControl[2].IsOn && TaskDrill.instance.MachineStabalized)
        {
            DriveFailure(5);
            return;
        }

        if (taskControl[0].IsOn && taskControl[1].IsOn)
        {
            if (taskControl[2].IsOn)
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
