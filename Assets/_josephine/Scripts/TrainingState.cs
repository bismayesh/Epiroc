using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingState : MonoBehaviour
{
    //Tasks
    public TaskDrive taskDrive;
    public TaskDrill taskDrill;
    public TaskLights taskLights;

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

    //Task figures
    int taskDriveProgress = 0;
    int taskDriveFailures = 0;
    int taskDrillProgress = 0;
    int taskDrillFailures = 0;
    int taskLightsProgress = 0;
    int taskLightsFailures = 0;

    //UI
    public TextMeshProUGUI textTrainingProgress;
    public TextMeshProUGUI textTrainingFailures;
    public TextMeshProUGUI textTrainingDamage;
    public TextMeshProUGUI textTrainingScore;
    public TextMeshProUGUI textTrainingScoreMultiplier;
    public TextMeshProUGUI textTaskDriveProgress;
    public TextMeshProUGUI textTaskDriveFailures;
    public TextMeshProUGUI textTaskDrillProgress;
    public TextMeshProUGUI textTaskDrillFailures;
    public TextMeshProUGUI textTaskLightsProgress;
    public TextMeshProUGUI textTaskLightsFailures;

    private void Start()
    {
        neededTrainingIterations = taskDrive.neededCheckpoints + taskDrill.neededIterations + taskLights.neededIterations;

        textTrainingProgress.text = "Progress: " + trainingProgress + "%";
        textTrainingFailures.text =  "Failures: " + trainingFailures;
        textTrainingDamage.text = "Machine damage: " + trainingDamage;
        textTrainingScore.text = "Training score: " + trainingScore;
        textTrainingScoreMultiplier.text = scoreMultiplier.ToString() + "X";

        textTaskDriveProgress.text = "Progress: " + taskDriveProgress + "%";
        textTaskDriveFailures.text = "Failures: " + taskDriveFailures;
    }

    public void UpdateTaskDriveProgress(int neededIt, int currentIt)
    {
        taskDriveProgress = (int)((float)currentIt / (float)neededIt * 100);
        textTaskDriveProgress.text = taskDriveProgress + "%";

        UpdateTrainingProgress();
    }

    public void UpdateTaskDrillProgress(int neededIt, int currentIt)
    {
        taskDriveProgress = (int)((float)currentIt / (float)neededIt * 100);
        textTaskDriveProgress.text = taskDriveProgress + "%";

        UpdateTrainingProgress();
    }

    public void UpdateTaskLightsProgress(int neededIt, int currentIt)
    {
        taskDriveProgress = (int)((float)currentIt / (float)neededIt * 100);
        textTaskDriveProgress.text = taskDriveProgress + "%";

        UpdateTrainingProgress();
    }

    public void UpdateTrainingFailures()
    {
        trainingFailures++;
        textTrainingFailures.text = "Failures: " + trainingFailures;
        scoreMultiplier = 1;
    }

    private void UpdateTrainingProgress()
    {
        currentTrainingIteration++;
        trainingProgress = (int)((float)currentTrainingIteration / (float)neededTrainingIterations);
        textTrainingProgress.text = "Progress: " + trainingProgress + "%";

        if (currentTrainingIteration == neededTrainingIterations)
        {
            Debug.Log("Training finnished!");
            Invoke("TrainingFinnished", 2);
        }
    }

    private void TrainingFinnished()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
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

    public void UpdateTrainingScore(int gemScore)
    {
        trainingScore += gemScore * scoreMultiplier;
        scoreMultiplier *= 2;
        textTrainingScore.text = "Score: " + trainingScore;
        textTrainingScoreMultiplier.text = scoreMultiplier.ToString() + "X";
    }
}