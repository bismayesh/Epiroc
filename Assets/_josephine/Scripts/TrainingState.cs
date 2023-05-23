using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingState : MonoBehaviour
{
    //public string trainingName;

    //Tasks
    public TaskStart taskStart;
    public TaskDrive taskDrive;
    public TaskDrill taskDrill;
    public TaskLights taskLights;
    public TaskStop taskStop;

    //Training figures
    int neededTrainingIterations;
    int currentTrainingIteration = 0;

    int trainingProgress = 0;
    int trainingFailures = 0;
    int trainingDamage = 0;
    int trainingScore = 0;
    int scoreMultiplier = 1;

    //StartMachine figures
    int taskStartProgress = 0;
    int taskStartFailures = 0;

    //Drive figures

    //

    //UI
    public TextMeshProUGUI textTrainingProgress;
    public TextMeshProUGUI textTrainingFailures;
    public TextMeshProUGUI textTrainingDamage;
    public TextMeshProUGUI textTrainingScore;
    public TextMeshProUGUI textTrainingScoreMultiplier;
    public TextMeshProUGUI textStartMachineProgress;
    public TextMeshProUGUI textStartMachineFailures;


    private void Start()
    {
        neededTrainingIterations = taskStart.neededIterations 
            + taskDrive.neededCheckpoints + taskDrill.neededIterations + 
            taskLights.neededIterations + taskStop.neededIterations;

        textTrainingProgress.text = "Progress: " + trainingProgress + "%";
        textTrainingFailures.text =  "Failures: " + trainingFailures;
        textTrainingDamage.text = "Machine damage: " + trainingDamage;
        textTrainingScore.text = "Training score: " + trainingScore;
        textTrainingScoreMultiplier.text = scoreMultiplier.ToString() + "X";

        textStartMachineProgress.text = "Progress: " + taskStartProgress + "%";
        textStartMachineFailures.text = "Failures: " + taskStartFailures;
    }

    public void UpdateStartMachineProgress(int neededIt, int currentIt)
    {
        taskStartProgress = (int)((float)currentIt / (float)neededIt * 100);
        textStartMachineProgress.text = taskStartProgress + "%";

        UpdateTrainingProgress();
    }

    public void UpdateTaskDriveProgress(int neededIt, int currentIt)
    {
        taskStartProgress = (int)((float)currentIt / (float)neededIt * 100);
        textStartMachineProgress.text = taskStartProgress + "%";

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