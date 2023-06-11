using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TaskTorch : Task
{
    //public AudioClip torchAudio = null;
    public List<SingleTask> taskControl = new List<SingleTask>();
    bool firstTime = true;

    public bool testDrive = false;

    //Singelton
    public static TaskTorch instance;

    private void Update()
    {
        if (testDrive)
        {
            machineController.ChangeMovementForce(new Vector2(1.0f, 1.0f));
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public bool LightMood
    {
        get { return taskControl[0].IsOn; }
    }

    public void FirstTorchInstructions()
    {
        if (firstTime)
        {
            firstTime = false;
            SupportLevels.instance.SupportInstructions(SupportMood.Torch);
        }
    }

    public void TaskCheck(GameObject thisObject, bool setOn)
    {
        if (taskControl[0].TaskCheck(thisObject, setOn))
            ActivateSwitch();
        if (taskControl[1].TaskCheck(thisObject, setOn))
            FucusButton();
        if (taskControl[2].TaskCheck(thisObject, setOn))
            LightBeamButton();
    }

    private void ActivateSwitch()
    {
        if (taskControl[0].IsOn)
        {
            machineController.ActivateTorch();
            machineController.torchSpread = 35.0f;

        }
        else
        {
            machineController.DeactivateTorch();
            machineController.torchSpread = 35.0f;
        }
    }

    private void FucusButton()
    {
        if (taskControl[1].IsOn)
        {
            machineController.torchIntensity = 1.0f;
        }
        else
        {
            machineController.torchIntensity = 0.5f;
        }
    }

    private void LightBeamButton()
    {
        if (taskControl[2].IsOn)
        {
            if (!taskControl[0].IsOn || !taskControl[0].IsOn)
            {
                TorchFailure();
                return;
            }

            if (TaskDrill.instance.MachineStabalized)
            {
                TorchFailure(5);
                return;
            }

            StartCoroutine(LightBeam());
            TrollKillTorch.instance.FireLightPulse();
            TrainingState.instance.UpdateScoreMultiplier();
        }
    }

    IEnumerator LightBeam()
    {
        machineController.torchIntensity = 10.0f;
        yield return new WaitForSeconds(1);
        machineController.torchIntensity = 1.0f;
    }

    public void TorchProgress()
    {
        currentIteration++;
        TrainingState.instance.UpdateTaskTorchProgress(neededIterations, currentIteration);
    }

    void TorchFailure(int damage = 0)
    {
        if (failRecorded)
            return;

        failRecorded = true;
        TaskFailure(damage);
        TrainingState.instance.UpdateTorchFailure();
    }
}


/*
     public void Torch(Vector2 rotation, GameObject thisObject)
    {
        if (!holdingJoystick)
        {
            //SetAudio(torchAudio, false);
            //machineController.ResetRotation();
            return;
        }


        if (TaskDrill.instance.MachineStabalized)
        {
            TorchFailure(5);
            return;
        }

        if (taskControl[0].IsOn && taskControl[1].IsOn && taskControl[2].IsOn)
        {
            //SetAudio(torchAudio, true);

            if (thisObject.CompareTag("LeftJoystick"))
            {
                machineController.ChangeTorchRotationY(rotation);
            }

            if (!thisObject.CompareTag("LeftJoystick"))
            {
                machineController.ChangeTorchRotationX(rotation);
            }
            
        }
        else
        {
            TorchFailure();
        }
    }
*/
