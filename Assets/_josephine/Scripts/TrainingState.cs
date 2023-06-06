using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingState : MonoBehaviour
{
    //public SmoothLocomotion smoothLocomotion;
    bool smoothLocomotionChanged = true;
    public Transform playerPosition;
    Vector3 playerStartPosition;

    //Training figures
    int neededTrainingIterations;
    int currentTrainingIteration = 0;

    int trainingProgress = 0;
    int trainingFailures = 0;
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

    [Header("Damage")]
    public Transform damageMeter;
    public Renderer damageIndicator;
    public Gradient damageColor;
    //public Gradient emissiveColor;
    [SerializeField]
    int maxDamage = 100;
    [SerializeField]
    int curDamage = 0;
    

    //UI
    [Header("Training info")]
    public TextMeshProUGUI textTrainingProgress;
    public TextMeshProUGUI textTrainingFailures;
    public TextMeshProUGUI textTrainingScore;
    public TextMeshProUGUI textTrainingScoreMultiplier;
    public TextMeshProUGUI textTrainingGemsCount;
    public TextMeshProUGUI textTrainingTrollsKill;

    [Header("Task info")]
    public TextMeshProUGUI textTaskDriveProgress;
    public TextMeshProUGUI textTaskDriveFailures;
    public TextMeshProUGUI textTaskDrillProgress;
    public TextMeshProUGUI textTaskDrillFailures;
    public TextMeshProUGUI textTaskTorchProgress;
    public TextMeshProUGUI textTaskTorchFailures;

    //Singelton
    public static TrainingState instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        playerStartPosition = playerPosition.localPosition;
        neededTrainingIterations = TaskDrive.instance.neededIterations + TaskDrill.instance.neededIterations + TaskTorch.instance.neededIterations;

        textTrainingProgress.text = "Progress: " + trainingProgress + "%";
        textTrainingFailures.text =  "Failures: " + trainingFailures;
        textTrainingScore.text = "Training score: " + trainingScore;
        textTrainingScoreMultiplier.text = "Multiplier: " + scoreMultiplier.ToString() + "X";

        textTaskDriveProgress.text = taskDriveProgress + "%";
        textTaskDriveFailures.text = taskDriveFailures.ToString();

        UpdateDamageMeter();
    }

    /*
    void LateUpdate()
    {
        if (smoothLocomotionChanged)
        {
            smoothLocomotionChanged = false;
            playerPosition.GetComponentInChildren<SmoothLocomotion>().enabled = false;
        }

        playerPosition.localPosition = playerStartPosition;
    }
    */

    public void UpdateTaskDriveProgress(int neededIt, int currentIt)
    {
        textTaskDriveProgress.text = Percentage(neededIt, currentIt).ToString() + "%";

        UpdateTrainingProgress();
    }

    public void UpdateTaskDrillProgress(int neededIt, int currentIt)
    {
        currentIt++;
        textTaskDrillProgress.text = Percentage(neededIt, currentIt).ToString() + "%";

        UpdateTrainingProgress();
    }

    public void UpdateTaskTorchProgress(int neededIt, int currentIt)
    {
        textTaskTorchProgress.text = Percentage(neededIt, currentIt).ToString() + "%";

        trollsKill++;
        textTrainingTrollsKill.text = "Trolls Kill: " + trollsKill.ToString();

        UpdateTrainingProgress();
    }

    int Percentage(int neededIt, int currentIt)
    {
        return (int)((float)currentIt / (float)neededIt * 100);
    }

    void UpdateTrainingProgress()
    {
        currentTrainingIteration++;
        trainingProgress = Percentage(neededTrainingIterations, currentTrainingIteration);
        textTrainingProgress.text = "Progress: " + trainingProgress + "%";

        if (currentTrainingIteration == neededTrainingIterations)
        {
            Debug.Log("Training finnished!");
            Invoke("TrainingFinnished", 2);
            //Spawn mega gem
        }
    }

    void TrainingFinnished()
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
        textTrainingScoreMultiplier.text = "Multiplier: " + scoreMultiplier.ToString() + "X";
        textTrainingGemsCount.text = "Gems: " + gemsCount.ToString();
    }

    public void UpdateTrainingFailures()
    {
        trainingFailures++;
        textTrainingFailures.text = "Failures: " + trainingFailures;
        scoreMultiplier = 1;
    }

    public void UpdateDriveFailure()
    {
        taskDriveFailures++;
        textTaskDriveFailures.text = taskDriveFailures.ToString();
    }

    public void UpdateDrillFailure()
    {
        taskDrillFailures++;
        textTaskDrillFailures.text = taskDrillFailures.ToString();
    }

    public void UpdateTorchFailure()
    {
        taskTorchFailures++;
        textTaskTorchFailures.text = taskTorchFailures.ToString();
    }

    public void MachineDamage(int damage)
    {
        curDamage += damage;
        UpdateDamageMeter();

        if (curDamage >= maxDamage)
        {
            trainingScore = 0;

            //You fired screen
        }
    }

    void UpdateDamageMeter()
    {
        
        float damageSize = 1.0f / (float)maxDamage;
        float meterSize = ((float)maxDamage - (float)curDamage) * damageSize;

        damageMeter.localScale = new Vector3(meterSize, 1, 1);
        damageIndicator.material.color = damageColor.Evaluate(meterSize);
        //damageIndicator.material.SetColor("_EmissiveColor", emissiveColor.Evaluate(meterSize) * 3.9f);
    }
}