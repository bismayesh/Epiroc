using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingState : MonoBehaviour
{
    public SmoothLocomotion smoothLocomotion;
    bool smoothLocomotionOn = false;

    //Tasks
    public TaskDrive taskDrive;
    public TaskDrill taskDrill;
    public TaskLight taskLight;

    //Training figures
    int neededTrainingIterations;
    int currentTrainingIteration = 0;
    [SerializeField]
    int maxDamage = 100;

    int trainingProgress = 0;
    int trainingFailures = 0;
    int trainingDamage = 0;
    int trainingScore = 0;
    int scoreMultiplier = 1;
    int gemsCount = 0;
    int trollsKill = 0;

    //Task figures
    int taskDriveProgress = 0;
    int taskDriveFailures = 0;
    int taskDrillProgress = 0;
    int taskDrillFailures = 0;
    int taskTorchProgress = 0;
    int taskTorchFailures = 0;

    //UI
    public TextMeshProUGUI textTrainingProgress;
    public TextMeshProUGUI textTrainingFailures;
    public TextMeshProUGUI textTrainingDamage;
    public TextMeshProUGUI textTrainingScore;
    public TextMeshProUGUI textTrainingScoreMultiplier;
    public TextMeshProUGUI textTrainingGemsCount;
    public TextMeshProUGUI textTrainingTrollsKill;
    public TextMeshProUGUI textTaskDriveProgress;
    public TextMeshProUGUI textTaskDriveFailures;
    public TextMeshProUGUI textTaskDrillProgress;
    public TextMeshProUGUI textTaskDrillFailures;
    public TextMeshProUGUI textTaskTorchProgress;
    public TextMeshProUGUI textTaskTorchFailures;

    private void LateUpdate()
    {
        if (!smoothLocomotionOn)
        {
            smoothLocomotionOn = true;
            smoothLocomotion.enabled = true;
        }
    }

    private void Start()
    {
        taskDrive = GameObject.Find("Drive").GetComponent<TaskDrive>();
        taskDrill = GameObject.Find("Drill").GetComponent<TaskDrill>();
        taskLight = GameObject.Find("Torch").GetComponent<TaskLight>();

        neededTrainingIterations = taskDrive.neededCheckpoints + taskDrill.neededIterations + taskLight.neededTrolls;

        textTrainingProgress.text = "Progress: " + trainingProgress + "%";
        textTrainingFailures.text =  "Failures: " + trainingFailures;
        textTrainingDamage.text = "Machine damage: " + trainingDamage;
        textTrainingScore.text = "Training score: " + trainingScore;
        textTrainingScoreMultiplier.text = scoreMultiplier.ToString() + "X";

        textTaskDriveProgress.text = taskDriveProgress + "%";
        textTaskDriveFailures.text = taskDriveFailures.ToString();
    }

    public void UpdateTaskDriveProgress(int neededIt, int currentIt)
    {
        taskDriveProgress = (int)((float)currentIt / (float)neededIt * 100);
        textTaskDriveProgress.text = taskDriveProgress + "%";

        UpdateTrainingProgress();
    }

    public void UpdateTaskDrillProgress(int neededIt, int currentIt)
    {
        taskDrillProgress = (int)((float)currentIt / (float)neededIt * 100);        
        textTaskDrillProgress.text = taskDrillProgress + "%";

        UpdateTrainingProgress();
    }

    public void UpdateTaskTorchProgress(int neededIt, int currentIt)
    {
        taskTorchProgress = (int)((float)currentIt / (float)neededIt * 100);
        textTaskTorchProgress.text = taskTorchProgress + "%";

        trollsKill++;
        textTrainingTrollsKill.text = trollsKill.ToString();

        UpdateTrainingProgress();
    }

    private void UpdateTrainingProgress()
    {
        currentTrainingIteration++;
        trainingProgress = (int)((float)currentTrainingIteration / (float)neededTrainingIterations * 100);
        textTrainingProgress.text = "Progress: " + trainingProgress + "%";

        if (currentTrainingIteration == neededTrainingIterations)
        {
            Debug.Log("Training finnished!");
            Invoke("TrainingFinnished", 2);
            //Spawn mega gem
        }
    }

    private void TrainingFinnished()
    {
        //SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void UpdateTrainingScore(int gemScore)
    {
        trainingScore += gemScore * scoreMultiplier;
        scoreMultiplier *= 2;
        gemsCount++;
        textTrainingScore.text = "Score: " + trainingScore;
        textTrainingScoreMultiplier.text = scoreMultiplier.ToString() + "X";
        textTrainingGemsCount.text = gemsCount.ToString();
    }

    public void UpdateTrainingFailures()
    {
        trainingFailures++;
        textTrainingFailures.text = "Failures: " + trainingFailures;
        scoreMultiplier = 1;
    }

    public int MachineDamage
    {
        get { return trainingDamage; }
        set
        {
            trainingDamage = value;
            textTrainingDamage.text = "Damage: " + trainingDamage;

            if (trainingDamage >= maxDamage)
            {
                trainingScore = 0;

                //You fired screen
            }
        }
    }

    private void UpdateDamageMeter()
    {

    }
}