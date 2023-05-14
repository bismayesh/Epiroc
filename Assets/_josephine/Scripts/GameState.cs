using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameState : MonoBehaviour
{
    public TextMeshProUGUI textTrainingName;
    public TextMeshProUGUI textTrainingProgress;
    public TextMeshProUGUI textTaskName;
    public TextMeshProUGUI texTaskProgress;
    public TextMeshProUGUI textFailures;
    public TextMeshProUGUI textAttempts;
    public TextMeshProUGUI textDamage;
    public TextMeshProUGUI textHP;

    private string currentTrainingName;
    private int currentTrainingProgress = 0;
    private string currentTaskName;
    private int currentTaskProgress = 0;
    private int currentFailure = 0;
    private int currentAttempt = 0;
    private int currentDamage = 0;
    [SerializeField]
    private int playerHP = 100;

    private void Start()
    {
        textTrainingProgress.text = "Progress: " + currentTrainingProgress + "%";
        texTaskProgress.text = "Progress: " + currentTaskProgress + "%";
        textFailures.text = "Failures: " + currentFailure;
        textAttempts.text = "Attempts: " + currentAttempt;
        textDamage.text = "Machine damage: " + currentDamage;
        textHP.text = "HP: " + playerHP;
    }

    public string TrainingName
    {
        get { return currentTrainingName; }
        set
        {
            currentTrainingName = value;
            textTrainingName.text = currentTrainingName;
        }
    }

    public int TrainingProgress
    {
        get { return currentTrainingProgress; }
        set
        {
            currentTrainingProgress = value;

            if (currentTrainingProgress >= 100)
            {
                //Finnished training condition
                textTrainingProgress.text = "Progress: " + currentTrainingProgress + "%";
            }
            else
            {
                //Progress increase condition
                textTrainingProgress.text = "Progress: " + currentTrainingProgress + "%";
            }
        }
    }

    public int TrainingAttempts
    {
        get { return currentAttempt; }
        set
        {
            currentAttempt = value;
            textAttempts.text = "Attempts: " + currentAttempt;
        }
    }

    public int TrainingFailures
    {
        get { return currentFailure; }
        set
        {
            currentFailure = value;
            textFailures.text = "Failures: " + currentFailure;
        }
    }

    public string TaskName
    {
        get { return currentTaskName; }
        set
        {
            currentTaskName = value;
            textTaskName.text = "Current task: " + currentTaskName;
        }
    }

    public int TaskProgress
    {
        get { return currentTaskProgress; }
        set
        {
            currentTaskProgress = value;

            if (currentTaskProgress >= 100)
            {
                //Finnished task condition
                texTaskProgress.text = "Progress: " + currentTaskProgress + "%";
            }
            else
            {
                //Progress increase condition
                texTaskProgress.text = "Progress: " + currentTaskProgress + "%";
            }
        }
    }

    public int MachineDamage
    {
        get { return currentDamage; }
        set
        {
            currentDamage = value;
            textDamage.text = "Damage: " + currentDamage;
        }
    }

    public int HP
    {
        get { return playerHP; }
        set
        {
            //Change value example:
            //private GameBehavior gameManager;
            //gameManager.HP -= 1;

            playerHP = value;

            if (playerHP <= 0)
            {
                //Player died condition
                textHP.text = "HP: " + playerHP;
            }
            else
            {
                //Hurt condition
                textHP.text = "HP: " + playerHP;
            }
        }
    }
}
