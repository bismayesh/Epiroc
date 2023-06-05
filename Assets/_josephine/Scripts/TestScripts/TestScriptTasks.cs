using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

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


[System.Serializable]
public class SupportSystem
{
    public List<SupportLayer> layers;
}


public class TestDriveTask : Task
{
    [Header("Events")]
    public UnityEvent onDriving;

    public List<SingleTask> taskControl = new List<SingleTask>();
    public AudioClip driveAudio;

    [HideInInspector]
    public TaskDrill taskDrill;

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


        if (TaskDrill.instance.MachineStabalized)
        {
            TaskFailure(5);
            return;
        }

        if (!taskControl[0].isOn && !taskControl[1].isOn)
        {
            if (taskControl[0].isOn)
            {
                //event driving
                onDriving.Invoke();
                SetAudio(driveAudio, true);
                machineController.ChangeMovementForce(force);
            }
        }
        else
        {
            TaskFailure();
        }
    }
}

public class Task : MonoBehaviour
{
    public AudioSource audioSource;

    [SerializeField]
    protected bool holdingJoystick = false;
    [SerializeField]
    bool failRecorded = false;

    [HideInInspector]
    public MachineController machineController;

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
        machineController = FindObjectOfType<MachineController>();
    }

    protected void SetAudio(AudioClip clip, bool setOn)
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

    protected void TaskFailure()
    {
        if (!failRecorded)
        {
            failRecorded = true;
            TrainingState.instance.UpdateTrainingFailures();
        }
    }

    protected void TaskFailure(int damage)
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
public class SingleTask
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
        isOn = !isOn;
    }
}

[System.Serializable]
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
