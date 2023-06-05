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
    bool firstTime = true;
    GemSpawnPoint gemSpawnPoint = null;
    [SerializeField]
    bool machineStabalized = false;
    bool holdingJoystick = false;
    bool failureRecorded = false;

    public TrainingState currentTraining;
    public MachineController machineController;
    public SupportLevels supportLevels;

    public AudioSource audioSource;
    public AudioClip jacksAudio;
    public AudioClip drillAudio;
    bool audioDrillOn = false;
    bool audioJacksOn = false;

    public int neededIterations = 5;
    int currentIteration = 0;

    //Singelton
    public static TaskDrill instance;

    public bool DrillMood
    {
        get { return jacksLeverOn; }
        private set { jacksLeverOn = value;}
    }

    public GemSpawnPoint GemSpawnObject
    {
        get { return gemSpawnPoint; }
        set { gemSpawnPoint = value; }
    }

    public bool SpawnArea
    {
        get { return spawnArea; }
        set 
        { 
            spawnArea = value; 
        }
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

        if (spawnArea && firstTime)
        {
            firstTime = false;
            supportLevels.DrillInstructions();
        }
    }

    private void Start()
    {
        currentTraining = GameObject.FindGameObjectWithTag("Training").GetComponent<TrainingState>();
        machineController = GameObject.Find("Machine").GetComponent<MachineController>();
        //machineController.onTrollsKill.AddListener(currentTraining.UpdateTaskTorchProgress(neededIterations, currentIteration));
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

                    if (!audioJacksOn)
                    {
                        audioJacksOn = true;
                        audioSource.clip = jacksAudio;
                        audioSource.PlayOneShot(jacksAudio);
                    }
                }
                else if (button == drillLever)
                {
                    if (jacksLeverOn && frontJacksUp && rearJacksUp)
                    {
                        drilling = true;

                        if (!audioDrillOn)
                        {
                            audioDrillOn = true;
                            audioSource.clip = drillAudio;
                            audioSource.Play();
                        }
                        
                        machineController.ActivateDrill();
                        machineController.SpinDrill();

                        if (spawnArea)
                        {
                            spawnArea = false;
                            currentTraining.UpdateTaskDrillProgress( neededIterations, currentIteration);
                            gemSpawnPoint.SpawnGems();
                            //Spawning trolls
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
                    audioJacksOn = false;
                    machineController.RetrieveJacks();
                }
                else if (button == drillLever)
                {
                    drilling = false;

                    if (audioDrillOn)
                    {
                        audioDrillOn = false;
                        audioSource.Stop();
                        audioSource.clip = null;
                    }

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
                if (jacksToggleOn)
                {
                    if (value.y >= 0.5f)
                    {
                        frontJacksUp = true;
                        textFrontJacks.text = "Front Jacks Up";
                    }
                    else if (value.y <= -0.5f)
                    {
                        frontJacksUp = false;
                        textFrontJacks.text = "Front Jacks Down";
                    }
                }
                else if(!jacksToggleOn)
                {
                    if (value.y >= 0.5f)
                    {
                        rearJacksUp = true;
                        textRearJacks.text = "Rear Jacks Up";
                    }
                    else if (value.y <= -0.5f)
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
