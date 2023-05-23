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
    public bool activateEngine = false;
    public bool EngineStarted = false;

    public TrainingState currentTraining;
    public MachineController machineController;

    public int neededIterations = 5;
    int currentIteration = 0;

    private void Start()
    {
        currentTraining = GameObject.FindGameObjectWithTag("Training").GetComponent<TrainingState>();
        //gameManager = GameObject.Find("GameManager").GetComponent<GameState>();
    }

    public void StartEngine(GameObject thisObject, bool isActive)
    {
        if (thisObject == activateEngine)
        {
            if (isActive)
            {
                if (button1activated && button2activated && button3activated)
                {
                    Debug.Log("EngineStarted");
                    EngineStarted = true;
                    machineController.ActivateEngine();

                    currentIteration++;
                    currentTraining.UpdateTaskLightsProgress(neededIterations, currentIteration);
                }
                else
                {
                    EngineStarted = false;
                    TaskFailure();
                }
            }
            else
            {
                EngineStarted = false;
            }
        }
    }

    void TaskFailure()
    {
        currentTraining.UpdateTrainingFailures();
        //Show ghost animation
    }

    public void ActivateButton(GameObject thisObject)
    {
        ButtonSwitch(thisObject, button1, ref button1activated);
        ButtonSwitch(thisObject, button2, ref button2activated);
        ButtonSwitch(thisObject, button3, ref button3activated);
    }

    private void ButtonSwitch(GameObject thisObject, GameObject button, ref bool activateButton)
    {

        if (thisObject == button)
        {
            if (!activateButton)
            {
                activateButton = true;
            }
            else
            {
                activateButton = false;
            }
        }
    }
}
