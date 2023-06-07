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
    public bool frontJacksUp = false;
    [SerializeField]
    public bool rearJacksUp = false;
    [SerializeField]
    bool spawnArea = false;
    bool firstTime = true;
    bool torchInstruction = false;
    bool firstTorchInstruction = true;
    GemSpawnPoint gemSpawnPoint = null;
    [SerializeField]
    bool machineStabalized = false;
    [SerializeField]
    bool spawnTrolls = false;

    //Singelton
    public static TaskDrill instance;

    public bool DrillMood
    {
        get { return taskControl[0].IsOn; }
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
            SupportLevels.instance.SupportInstructions(SupportMood.Drill);
        }
    }

    void MachineStabalised()
    {
        if (!taskControl[0].IsOn && !taskControl[1].IsOn && !taskControl[2].IsOn)
        {
            machineStabalized = false;

            if (spawnTrolls)
            {
                spawnTrolls = false;

                if (firstTorchInstruction)
                {
                    firstTorchInstruction = false;
                    TaskTorch.instance.FirstTorchInstructions();
                }
                else
                {
                    TrollSpawner.instance.InstanciateTrolls();
                }
            }
        }
        else
        {
            machineStabalized = true;
        }
    }

    public void TaskCheck(GameObject thisObject, bool setOn)
    {
        if (taskControl[0].TaskCheck(thisObject, setOn))
            JacksLever();
        if (taskControl[1].TaskCheck(thisObject, setOn))
            JacksToggle();
        if (taskControl[2].TaskCheck(thisObject, setOn))
            DrillLever();
    }

    private void JacksLever()
    {
        if (taskControl[0].IsOn)
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
        if (taskControl[1].IsOn)
        {
            
        }
        else
        {
            
        }
    }

    private void DrillLever()
    {
        if (taskControl[2].IsOn)
        {
            if (taskControl[0].IsOn && frontJacksUp && rearJacksUp)
            {
                SetAudio(drillAudio[1], true, false);
                machineController.ActivateDrill();
                machineController.SpinDrill();

                if (spawnArea)
                {
                    spawnArea = false;
                    TrainingState.instance.UpdateTaskDrillProgress(neededIterations, currentIteration);
                    gemSpawnPoint.SpawnGems();
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

        if (TaskTorch.instance.taskControl[2].IsOn)
        {
            DrillFailure(3);
            return;
        }

        if (taskControl[0].IsOn)
        {
            if (taskControl[1].IsOn)
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
