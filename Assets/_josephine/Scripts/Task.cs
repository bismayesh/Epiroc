using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Task : MonoBehaviour
{
    public int neededIterations;
    public int currentIteration;
    public AudioSource audioSource;
    bool audioIsOn = false;

    protected bool holdingJoystick = false;
    public bool failRecorded = false;

    [HideInInspector]
    public MachineController machineController;

    public bool HoldingJoystick
    {
        get { return holdingJoystick; } 
        set { holdingJoystick = value; }
    }

    private void Update()
    {
        if (!holdingJoystick)
            failRecorded = false;
    }

    private void Start()
    {
        machineController = FindObjectOfType<MachineController>();
    }

    protected void SetAudio(AudioClip clip, bool setOn, bool oneTime)
    {
        if (!oneTime)
        {
            if (setOn && !audioIsOn)
            {
                audioIsOn = true;
                audioSource.clip = clip;
                audioSource.Play();
            }
            if (!setOn && audioIsOn)
            {
                audioIsOn = false;
                audioSource.Stop();
                audioSource.clip = null;
            }
        }
        if (oneTime)
        {
            if (setOn && !audioIsOn)
            {
                audioIsOn = true;
                audioSource.clip = null;
                audioSource.PlayOneShot(clip);
            }
            if (!setOn && audioIsOn)
            {
                audioIsOn = false;
            }
        }
    }

    protected void TaskFailure(int damage = 0)
    {
        TrainingState.instance.UpdateTrainingFailures();
        TrainingState.instance.MachineDamage(damage);
    }
}

[System.Serializable]
public class SingleTask
{
    public GameObject task;
    [SerializeField]
    private bool isOn = false;

    public GameObject supportText;
    public GameObject supportTextSmall;
    public GameObject supportLight;
    public AudioClip supportVoice;
    public GameObject supportGhost;

    public bool IsOn
    {
        get { return isOn; }
        protected set { isOn = value; }
    }

    public bool TaskCheck(GameObject thisObject, bool setOn)
    {
        if (thisObject == task && setOn)
        {
            //ButtonSwitch();
            isOn = true;
            return true;
        }
        if (thisObject == task && !setOn)
        {
            isOn = false;
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
