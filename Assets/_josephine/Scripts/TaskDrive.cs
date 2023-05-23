using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskDrive : MonoBehaviour
{
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public GameObject activateEngine;

    public bool button1activated = false;
    public bool button2activated = false;
    public bool button3activated = false;
    public bool EngineStarted = false;

    public TrainingState currentTraining;
    public MachineController machineController;

    public int neededCheckpoints = 5;
    int currentCheckpoint = 0;

    public int CurrentCheckpoint
    {
        get { return currentCheckpoint; }
        set 
        { 
            currentCheckpoint = value;
            currentTraining.UpdateTaskDriveProgress( neededCheckpoints, currentCheckpoint);
        }
    }
}
