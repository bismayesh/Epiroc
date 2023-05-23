using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskLights : MonoBehaviour
{
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public GameObject activateLights;

    public bool button1activated = false;
    public bool button2activated = false;
    public bool button3activated = false;
    public bool LightsOn = false;

    public TrainingState currentTraining;
    public MachineController machineController;

    public int neededIterations = 5;
    int currentIteration = 0;
}
