using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestScriptTasks : MonoBehaviour
{

}

public enum SupportLayer
{
    Text,
    Light,
    Voice,
    Ghost
}

public class DriveTask : MonoBehaviour
{
    public List<Task> taskControl = new List<Task>();

    public AudioSource audioSource;
    public AudioClip driveAudio;
    bool audioOn = false;

    [SerializeField]
    bool holdingJoystick = false;
    [SerializeField]
    bool failRecorded = false;

    [HideInInspector]
    public MachineController machineController;
    [HideInInspector]
    public TaskDrill taskDrill;

    public bool HoldingJoystick
    {
        get { return holdingJoystick; } 
        set
        {
            holdingJoystick = value;
            if (!holdingJoystick)
            {
                failRecorded = false;
            }
        }
    }


    private void Start()
    {
        machineController = GameObject.FindGameObjectWithTag("Machine").GetComponent<MachineController>();
        taskDrill = GameObject.Find("Drill").GetComponent<TaskDrill>();
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



    }

    public void Drive(Vector2 force)
    {
        if (!holdingJoystick)
        {
            SetAudio(driveAudio, false);
            return;
        }
            

        if (taskDrill.MachineStabalized)
        {
            TaskFailure(5);
            return;
        }

        if (!taskControl[0].isOn && !taskControl[1].isOn)
        {
            if (taskControl[0].isOn)
            {
                SetAudio(driveAudio, true);
                machineController.ChangeMovementForce(force); 
            }
        }
        else
        {
            TaskFailure();
        }
    }

    private void SetAudio(AudioClip clip, bool setOn)
    {
        if (setOn)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
    }

    private void TaskFailure()
    {
        if (!failRecorded)
        {
            failRecorded = true;
            TrainingState.instance.UpdateTrainingFailures();
        }
    }

    private void TaskFailure(int damage)
    {
        if (!failRecorded)
        {
            failRecorded = true;
            TrainingState.instance.UpdateTrainingFailures();
            TrainingState.instance.MachineDamage += damage;
        }
    }
}



[System.Serializable]
public class SupportSystem
{
    public List<SupportLayer> layers;
}


[System.Serializable]
public class Task
{
    public GameObject task;
    public bool isOn = false;

    public GameObject supportLight;
    public GameObject supportVoice;
    public GameObject supportGhost;

    public bool TaskCheck(GameObject thisObject)
    {
        if (thisObject == task)
        {
            ButtonSwitch();
            return true;
        }
        return false;
    }

    private void ButtonSwitch()
    {
        if (!isOn)
        {
            isOn = true;
        }
        else
        {
            isOn = false;
        }
    }
}

public class TextTask
{
    public TextMeshProUGUI textTask;
    public string taskMessage;
    public GameObject supportLight;
    public GameObject supportVoice;
    public GameObject supportGhost;

    bool taskComplete = false;

    void Update()
    {
        if (textTask.text == taskMessage)
        {
            taskComplete = true;
        }
    }

    public bool TaskComplete()
    {
        if (taskComplete)
        {
            return true;
        }
        return false;
    }
}
