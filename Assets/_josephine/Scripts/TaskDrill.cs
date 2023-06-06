using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using static UnityEngine.Rendering.DebugUI;

public class TaskDrill : Task
{
    public List<AudioClip> drillAudio = new List<AudioClip>();
    public List<SingleTask> taskControl = new List<SingleTask>();

    [Header("Jacks")]
    public TextMeshProUGUI textFrontJacks;
    public TextMeshProUGUI textRearJacks;
    [SerializeField]
    bool frontJacksUp = false;
    [SerializeField]
    bool rearJacksUp = false;
    [SerializeField]
    bool spawnArea = false;
    bool firstTime = true;
    bool torchInstruction = false;
    bool instructionShowed = false;
    GemSpawnPoint gemSpawnPoint = null;
    [SerializeField]
    bool machineStabalized = false;
    [SerializeField]
    bool spawnTrolls = false;

    //Singelton
    public static TaskDrill instance;

    public bool DrillMood
    {
        get { return taskControl[0].isOn; }
        private set { taskControl[0].isOn = value; }
    }

    public bool MachineStabalized
    {
        get { return machineStabalized; }
        private set { machineStabalized = value; }
    }

    public GemSpawnPoint GemSpawnObject
    {
        get { return gemSpawnPoint; }
        set { gemSpawnPoint = value; }
    }

    public bool SpawnArea
    {
        get { return spawnArea; }
        set { spawnArea = value; }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        FirstDrillInstructions();
        MachineStabalised();
    }

    void FirstDrillInstructions()
    {
        if (spawnArea && firstTime)
        {
            firstTime = false;
            SupportLevels.instance.DrillInstructions();
        }
    }

    void MachineStabalised()
    {
        if (!taskControl[0].isOn && !taskControl[1].isOn && !taskControl[2].isOn)
        {
            machineStabalized = false;

            if (torchInstruction && !instructionShowed)
            {
                instructionShowed = true;
                TaskTorch.instance.FirstTorchInstructions();
            }
            else if (instructionShowed && spawnTrolls)
            {
                spawnTrolls = false;
                TrollSpawner.instance.InstanciateTrolls();
            }
        }
        else
        {
            machineStabalized = true;
        }
    }

    public void TaskCheck(GameObject thisObject)
    {
        if (taskControl[0].TaskCheck(thisObject))
            JacksLever();
        if (taskControl[1].TaskCheck(thisObject))
            JacksToggle();
        if (taskControl[2].TaskCheck(thisObject))
            DrillLever();
    }

    private void JacksLever()
    {
        if (taskControl[0].isOn)
        {
            SetAudio(drillAudio[0], true, true);
            machineController.ExtendJacks();
        }
        else
        {
            SetAudio(drillAudio[0], false, true);
            machineController.RetrieveJacks();
        }
    }

    private void JacksToggle()
    {
        if (taskControl[1].isOn)
        {
            
        }
        else
        {
            
        }
    }

    private void DrillLever()
    {
        if (taskControl[2].isOn)
        {
            if (taskControl[0].isOn && frontJacksUp && rearJacksUp)
            {
                SetAudio(drillAudio[1], true, false);
                machineController.ActivateDrill();
                machineController.SpinDrill();

                if (spawnArea)
                {
                    spawnArea = false;
                    TrainingState.instance.UpdateTaskDrillProgress(neededIterations, currentIteration);
                    gemSpawnPoint.SpawnGems();
                    torchInstruction = true;
                    spawnTrolls = true;
                }
            }
            else
            {
                DrillFailure(5);
                failRecorded = false;
            }
        }
        else
        {
            SetAudio(drillAudio[1], false, false);
            machineController.DeactivateDrill();
            machineController.StopDrill();
        }
    }

    public void Drill(Vector2 value)
    {
        if (!holdingJoystick)
        {
            return;
        }

        if (TaskTorch.instance.taskControl[2].isOn)
        {
            DrillFailure(3);
            return;
        }

        if (taskControl[0].isOn)
        {
            Debug.Log(value);

            if (taskControl[1].isOn)
            {
                if (value.y >= 0.4f)
                {
                    frontJacksUp = true;
                    textFrontJacks.text = "Front Jacks Up";
                }
                else if (value.y <= -0.4f)
                {
                    frontJacksUp = false;
                    textFrontJacks.text = "Front Jacks Down";
                }
            }
            else
            {
                if (value.y >= 0.4f)
                {
                    rearJacksUp = true;
                    textRearJacks.text = "Rear Jacks Up";
                }
                else if (value.y <= -0.4f)
                {
                    rearJacksUp = false;
                    textRearJacks.text = "Rear Jacks Down";
                }
            }
        }
    }

    public void DrillProgress()
    {
        currentIteration++;
        TrainingState.instance.UpdateTaskDrillProgress(neededIterations, currentIteration);
    }

    void DrillFailure(int damage = 0)
    {
        if (failRecorded)
            return;

        failRecorded = true;
        TaskFailure(damage);
        TrainingState.instance.UpdateDrillFailure();
    }
}
