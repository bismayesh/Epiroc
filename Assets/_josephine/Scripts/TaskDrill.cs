using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TaskDrill : MonoBehaviour
{
    public GameObject jacksLever;
    public GameObject jacksToggle;
    public GameObject drillLever;

    public TextMeshProUGUI textFrontJacks;
    public TextMeshProUGUI textRearJacks;

    [SerializeField]
    bool jacksLeverOn = false;
    [SerializeField]
    bool jacksToggleOn = false;
    [SerializeField]
    bool frontJacksUp = false;
    [SerializeField]
    bool rearJacksUp = false;
    [SerializeField]
    bool drillLeverOn = false;
    [SerializeField]
    bool drilling = false;
    [SerializeField]
    bool spawnArea = false;
    [SerializeField]
    bool machineStabalized = false;
    bool holdingJoystick = false;
    bool failureRecorded = false;

    public TrainingState currentTraining;
    public MachineController machineController;

    public int neededIterations = 5;
    int currentIteration = 0;

    public bool DrillMood
    {
        get { return jacksLeverOn; }
        private set { jacksLeverOn = value;}
    }

    public bool SpawnArea
    {
        get { return spawnArea; }
        set { spawnArea = value; }
    }

    public bool MachineStabalized
    {
        get { return machineStabalized; }
        private set { machineStabalized = value; }
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

    private void FixedUpdate()
    {
        if (!jacksLeverOn && !frontJacksUp && !rearJacksUp && !drillLeverOn)
        {
            machineStabalized = false;
        }
        else
        {
            machineStabalized = true;
        }
    }

    public void ActivateButton(GameObject thisObject)
    {
        ButtonSwitch(thisObject, jacksLever, ref jacksLeverOn);
        ButtonSwitch(thisObject, jacksToggle, ref jacksToggleOn);
        ButtonSwitch(thisObject, drillLever, ref drillLeverOn);
    }

    private void ButtonSwitch(GameObject thisObject, GameObject button, ref bool activateButton)
    {
        if (thisObject == button)
        {
            if (!activateButton)
            {
                activateButton = true;

                if (button == jacksLever)
                {
                    machineController.ExtendJacks();
                }
                else if (button == drillLever)
                {
                    if (jacksLeverOn && frontJacksUp && rearJacksUp)
                    {
                        Debug.Log("Drilling");
                        drilling = true;
                        machineController.ActivateDrill();
                        machineController.SpinDrill();

                        if (spawnArea)
                        {
                            spawnArea = false;
                            currentTraining.UpdateTaskDrillProgress( neededIterations, currentIteration);
                            //Spawning gems
                            //Gems trigger area destroyed
                        }
                    }
                    else
                    {
                        TaskFailure();
                    }
                }
            }
            else
            {
                activateButton = false;
                if (button == jacksLever)
                {
                    machineController.RetrieveJacks();
                }
                else if (button == drillLever)
                {
                    drilling = false;
                    machineController.DeactivateDrill();
                    machineController.StopDrill();
                }  
            }
        }
    }

    public void Drill(Vector2 value)
    {
        if (holdingJoystick)
        {
            if (jacksLeverOn)
            {
                Debug.Log("info comming through");
                if (jacksToggleOn)
                {
                    if (value.y >= 0.8f)
                    {
                        frontJacksUp = true;
                        textFrontJacks.text = "Front Jacks Up";
                    }
                    else if (value.y <= -0.8f)
                    {
                        frontJacksUp = false;
                        textFrontJacks.text = "Front Jacks Down";
                    }
                }
                else if(!jacksToggleOn)
                {
                    if (value.y >= 0.8f)
                    {
                        rearJacksUp = true;
                        textRearJacks.text = "Rear Jacks Up";
                    }
                    else if (value.y <= -0.8f)
                    {
                        rearJacksUp = false;
                        textRearJacks.text = "Rear Jacks Down";
                    }
                }
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
    }

    void TaskFailure()
    {
        Debug.Log("Drill task fail!");
        currentTraining.UpdateTrainingFailures();
        //Show ghost animation
    }
}
