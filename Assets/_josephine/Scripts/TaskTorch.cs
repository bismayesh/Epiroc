using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TaskTorch : Task
{
    //public AudioClip torchAudio = null;
    public List<SingleTask> taskControl = new List<SingleTask>();
    bool firstTime = true;

    //Singelton
    public static TaskTorch instance;

    private void Awake()
    {
        instance = this;
    }

    public bool LightMood
    {
        get { return taskControl[0].isOn; }
        private set { taskControl[0].isOn = value; }
    }

    public void FirstTorchInstructions()
    {
        if (firstTime)
        {
            firstTime = false;
            SupportLevels.instance.TorchInstructions();
        }
    }

    public void TaskCheck(GameObject thisObject)
    {
        if (taskControl[0].TaskCheck(thisObject))
            ActivateButton();
        if (taskControl[1].TaskCheck(thisObject))
            FucusSlider();
        if (taskControl[2].TaskCheck(thisObject))
            IntensitySlider();
    }

    private void ActivateButton()
    {
        if (taskControl[0].isOn)
        {
            machineController.ActivateTorch();
        }
        else
        {
            machineController.DeactivateTorch();
        }
    }
    
    private void FucusSlider()
    {
        if (taskControl[1].isOn)
        {
            machineController.torchSpread = 0.0f;
        }
        else
        {
            machineController.torchSpread = 10.0f;
        }
    }

    private void IntensitySlider()
{
        if (taskControl[2].isOn)
        {
            machineController.torchIntensity = 5.0f;
        }
        else
        {
            machineController.torchIntensity = 0.5f;
        }
    }

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

        if (taskControl[0].isOn && taskControl[1].isOn && taskControl[2].isOn)
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
